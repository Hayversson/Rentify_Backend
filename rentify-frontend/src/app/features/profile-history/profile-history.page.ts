import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { Branch } from '../../core/models/branch.model';
import { Customer } from '../../core/models/customer.model';
import { Rental } from '../../core/models/rental.model';
import { RentalStatus } from '../../core/models/status.model';
import { Vehicle } from '../../core/models/vehicle.model';
import { BranchesService } from '../../core/services/branches.service';
import { CustomersService } from '../../core/services/customers.service';
import { RentalsService } from '../../core/services/rentals.service';
import { VehiclesService } from '../../core/services/vehicles.service';

interface RentalView {
  id: number;
  vehicleLabel: string;
  period: string;
  status: RentalStatus;
  totalCost: number;
  pickupBranch: string;
  returnBranch: string;
}

@Component({
  selector: 'app-profile-history-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile-history.page.html',
  styleUrl: './profile-history.page.scss'
})
export class ProfileHistoryPageComponent implements OnInit {
  protected readonly customers = signal<Customer[]>([]);
  protected readonly rentals = signal<RentalView[]>([]);
  protected readonly vehicles = signal<Vehicle[]>([]);
  protected readonly branches = signal<Branch[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal<string | null>(null);

  selectedCustomerId: number | null = null;

  constructor(
    private customersService: CustomersService,
    private rentalsService: RentalsService,
    private vehiclesService: VehiclesService,
    private branchesService: BranchesService
  ) {}

  ngOnInit(): void {
    forkJoin({
      customers: this.customersService.getCustomers(),
      vehicles: this.vehiclesService.getVehicles(),
      branches: this.branchesService.getBranches()
    }).subscribe({
      next: ({ customers, vehicles, branches }) => {
        this.customers.set(customers);
        this.vehicles.set(vehicles);
        this.branches.set(branches);
        this.selectedCustomerId = customers.length ? customers[0].id : null;

        if (this.selectedCustomerId) {
          this.loadRentals(this.selectedCustomerId);
        } else {
          this.isLoading.set(false);
        }
      },
      error: () => {
        this.errorMessage.set('No se pudieron cargar los datos de clientes.');
        this.isLoading.set(false);
      }
    });
  }

  loadRentals(customerId: number): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.rentalsService.getRentalsByCustomer(customerId).subscribe({
      next: (rentals) => {
        this.rentals.set(rentals.map((rental) => this.toRentalView(rental)));
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('No se pudo cargar el historial de rentas.');
        this.isLoading.set(false);
      }
    });
  }

  onCustomerChange(): void {
    if (this.selectedCustomerId) {
      this.loadRentals(this.selectedCustomerId);
    }
  }

  getStatusLabel(status: RentalStatus): string {
    switch (status) {
      case RentalStatus.Pending:
        return 'Pendiente';
      case RentalStatus.Active:
        return 'Activa';
      case RentalStatus.Completed:
        return 'Finalizada';
      case RentalStatus.Cancelled:
        return 'Cancelada';
      default:
        return 'Desconocida';
    }
  }

  get activeRentals(): RentalView[] {
    return this.rentals().filter((rental) =>
      [RentalStatus.Pending, RentalStatus.Active].includes(rental.status)
    );
  }

  get pastRentals(): RentalView[] {
    return this.rentals().filter((rental) =>
      [RentalStatus.Completed, RentalStatus.Cancelled].includes(rental.status)
    );
  }

  private toRentalView(rental: Rental): RentalView {
    const vehicle = this.vehicles().find((item) => item.id === rental.vehicleId);
    const pickupBranch = this.branches().find((item) => item.id === rental.pickupBranchId);
    const returnBranch = this.branches().find((item) => item.id === rental.returnBranchId);

    return {
      id: rental.id,
      vehicleLabel: vehicle ? `${vehicle.model} · ${vehicle.plate}` : `Vehículo ${rental.vehicleId}`,
      period: `${new Date(rental.startDate).toLocaleDateString()} · ${new Date(
        rental.endDate
      ).toLocaleDateString()}`,
      status: rental.status,
      totalCost: rental.totalCost,
      pickupBranch: pickupBranch?.name ?? 'Sucursal no disponible',
      returnBranch: returnBranch?.name ?? 'Sucursal no disponible'
    };
  }
}
