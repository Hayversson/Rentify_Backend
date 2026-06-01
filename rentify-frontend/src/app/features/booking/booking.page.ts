import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Branch } from '../../core/models/branch.model';
import { Customer } from '../../core/models/customer.model';
import { RentalRequest } from '../../core/models/rental.model';
import { VehicleType } from '../../core/models/vehicle-type.model';
import { Vehicle } from '../../core/models/vehicle.model';
import { VehicleStatus } from '../../core/models/status.model';
import { BranchesService } from '../../core/services/branches.service';
import { CustomersService } from '../../core/services/customers.service';
import { RentalsService } from '../../core/services/rentals.service';
import { VehicleTypesService } from '../../core/services/vehicle-types.service';
import { VehiclesService } from '../../core/services/vehicles.service';

@Component({
  selector: 'app-booking-page',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './booking.page.html',
  styleUrl: './booking.page.scss'
})
export class BookingPageComponent implements OnInit {
  protected readonly currentStep = signal(1);
  protected readonly steps = [
    { id: 1, title: 'Cliente & fechas' },
    { id: 2, title: 'Vehículo & sucursales' },
    { id: 3, title: 'Pago & confirmación' }
  ];

  protected readonly customers = signal<Customer[]>([]);
  protected readonly vehicles = signal<Vehicle[]>([]);
  protected readonly branches = signal<Branch[]>([]);
  protected readonly selectedVehicleBranch = signal<Branch | null>(null);
  protected readonly vehicleTypes = signal<VehicleType[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal<string | null>(null);
  protected readonly successMessage = signal<string | null>(null);
  protected readonly isSubmitting = signal(false);

  readonly form;

  constructor(
    private fb: FormBuilder,
    private customersService: CustomersService,
    private vehiclesService: VehiclesService,
    private branchesService: BranchesService,
    private vehicleTypesService: VehicleTypesService,
    private rentalsService: RentalsService
  ) {
    this.form = this.fb.group({
      customerId: [null as number | null, Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      vehicleId: [null as number | null, Validators.required],
      pickupBranchId: [null as number | null, Validators.required],
      returnBranchId: [null as number | null, Validators.required],
      paymentMethod: ['Card', Validators.required]
    });
  }

  ngOnInit(): void {
    this.form.get('vehicleId')?.valueChanges.subscribe((selectedVehicleIdRaw) => {
      const selectedVehicleId = typeof selectedVehicleIdRaw === 'string' && selectedVehicleIdRaw !== ''
        ? Number(selectedVehicleIdRaw)
        : selectedVehicleIdRaw;

      const selectedVehicle = this.vehicles().find((vehicle) => vehicle.id === selectedVehicleId);
      const branch = selectedVehicle ? this.branches().find((item) => item.id === selectedVehicle.branchId) ?? null : null;

      this.selectedVehicleBranch.set(branch);

      if (selectedVehicle) {
        this.form.patchValue({
          pickupBranchId: selectedVehicle.branchId,
          returnBranchId: selectedVehicle.branchId
        }, { emitEvent: false });
      } else {
        this.form.patchValue({
          pickupBranchId: null,
          returnBranchId: null
        }, { emitEvent: false });
      }
    });

    forkJoin({
      customers: this.customersService.getCustomers(),
      vehicles: this.vehiclesService.getVehicles(),
      branches: this.branchesService.getActiveBranches(),
      types: this.vehicleTypesService.getVehicleTypes()
    }).subscribe({
      next: ({ customers, vehicles, branches, types }) => {
        this.customers.set(customers);
        this.vehicles.set(vehicles);
        this.branches.set(branches);
        this.vehicleTypes.set(types);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('No se pudieron cargar los datos para la reserva.');
        this.isLoading.set(false);
      }
    });
  }

  setStep(step: number): void {
    this.currentStep.set(step);
  }

  nextStep(): void {
    this.currentStep.update((value) => Math.min(value + 1, 3));
  }

  previousStep(): void {
    this.currentStep.update((value) => Math.max(value - 1, 1));
  }

  submitReservation(): void {
    this.successMessage.set(null);
    this.errorMessage.set(null);

    if (this.form.invalid) {
      this.errorMessage.set('Completa todos los campos obligatorios antes de continuar.');
      return;
    }

    const value = this.form.getRawValue();
    
    // Validate dates
    const startDate = new Date(value.startDate!);
    const endDate = new Date(value.endDate!);
    
    if (isNaN(startDate.getTime()) || isNaN(endDate.getTime())) {
      this.errorMessage.set('Las fechas deben ser válidas.');
      return;
    }
    
    if (startDate >= endDate) {
      this.errorMessage.set('La fecha de inicio debe ser anterior a la de fin.');
      return;
    }
    
    if (startDate < new Date()) {
      this.errorMessage.set('La fecha de inicio no puede ser en el pasado.');
      return;
    }

    const payload: RentalRequest = {
      customerId: value.customerId!,
      startDate: value.startDate!,
      endDate: value.endDate!,
      vehicleId: value.vehicleId!,
      pickupBranchId: value.pickupBranchId!,
      returnBranchId: value.returnBranchId!
    };

    this.isSubmitting.set(true);
    this.rentalsService.createRental(payload, value.paymentMethod!).subscribe({
      next: (rental) => {
        this.successMessage.set(`Reserva creada con ID ${rental.id}.`);
        this.form.reset();
        this.currentStep.set(1);
        this.isSubmitting.set(false);
      },
      error: (error) => {
        // Try to extract detailed error message from backend
        let errorMsg = 'No se pudo crear la reserva. Revisa los datos e intenta nuevamente.';
        
        if (error?.error) {
          if (typeof error.error === 'string') {
            errorMsg = error.error;
          } else if (error.error?.message) {
            errorMsg = error.error.message;
          }
        }
        
        console.error('Booking error:', error);
        this.errorMessage.set(errorMsg);
        this.isSubmitting.set(false);
      }
    });
  }

  private getSelectedVehicle(): Vehicle | null {
    const vehicleIdRaw = this.form.get('vehicleId')?.value;
    const vehicleId = typeof vehicleIdRaw === 'string' && vehicleIdRaw !== ''
      ? Number(vehicleIdRaw)
      : vehicleIdRaw;

    if (vehicleId == null) {
      return null;
    }

    return this.vehicles().find((item) => item.id === vehicleId) ?? null;
  }

  getSelectedVehicleType(): VehicleType | null {
    const vehicle = this.getSelectedVehicle();
    if (!vehicle) {
      return null;
    }
    return this.vehicleTypes().find((type) => type.id === vehicle.vehicleTypeId) ?? null;
  }

  getFilteredVehicles(): Vehicle[] {
    const pickupBranchIdRaw = this.form.get('pickupBranchId')?.value;
    const pickupBranchId = typeof pickupBranchIdRaw === 'string'
      ? pickupBranchIdRaw === ''
        ? null
        : Number(pickupBranchIdRaw)
      : pickupBranchIdRaw;

    const isBranchSelected = pickupBranchId !== null && pickupBranchId !== undefined;

    return this.vehicles().filter((vehicle) => {
      const vehicleStatus = typeof vehicle.status === 'string'
        ? VehicleStatus[vehicle.status as keyof typeof VehicleStatus]
        : vehicle.status;

      const isAvailable = vehicleStatus === VehicleStatus.Available;
      const matchesBranch = !isBranchSelected || vehicle.branchId === pickupBranchId;
      return isAvailable && matchesBranch;
    });
  }

  getAvailableBranches(): Branch[] {
    const selectedBranch = this.selectedVehicleBranch();
    return selectedBranch ? [selectedBranch] : this.branches();
  }

  getEstimatedTotal(): number | null {
    const type = this.getSelectedVehicleType();
    const startDate = this.form.get('startDate')?.value;
    const endDate = this.form.get('endDate')?.value;

    if (!type || !startDate || !endDate) {
      return null;
    }

    const start = new Date(startDate);
    const end = new Date(endDate);
    const diff = Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
    const days = Number.isFinite(diff) && diff > 0 ? diff : 0;

    return days > 0 ? days * type.pricePerDay : null;
  }
}
