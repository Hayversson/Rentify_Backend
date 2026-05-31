import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { forkJoin, of, switchMap } from 'rxjs';
import { Branch } from '../../core/models/branch.model';
import { Vehicle } from '../../core/models/vehicle.model';
import { VehicleType } from '../../core/models/vehicle-type.model';
import { VehicleStatus } from '../../core/models/status.model';
import { BranchesService } from '../../core/services/branches.service';
import { VehicleTypesService } from '../../core/services/vehicle-types.service';
import { VehiclesService } from '../../core/services/vehicles.service';

@Component({
  selector: 'app-vehicle-detail-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './vehicle-detail.page.html',
  styleUrl: './vehicle-detail.page.scss'
})
export class VehicleDetailPageComponent implements OnInit {
  protected readonly vehicle = signal<Vehicle | null>(null);
  protected readonly vehicleType = signal<VehicleType | null>(null);
  protected readonly branch = signal<Branch | null>(null);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal<string | null>(null);

  constructor(
    private route: ActivatedRoute,
    private vehiclesService: VehiclesService,
    private vehicleTypesService: VehicleTypesService,
    private branchesService: BranchesService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.errorMessage.set('No se encontró el vehículo solicitado.');
      this.isLoading.set(false);
      return;
    }

    this.vehiclesService
      .getVehicleById(id)
      .pipe(
        switchMap((vehicle) =>
          forkJoin({
            vehicle: of(vehicle),
            type: this.vehicleTypesService.getVehicleTypeById(vehicle.vehicleTypeId),
            branch: this.branchesService.getBranchById(vehicle.branchId)
          })
        )
      )
      .subscribe({
        next: ({ vehicle, type, branch }) => {
          this.vehicle.set(vehicle);
          this.vehicleType.set(type);
          this.branch.set(branch);
          this.isLoading.set(false);
        },
        error: () => {
          this.errorMessage.set('No se pudieron cargar los datos del vehículo.');
          this.isLoading.set(false);
        }
      });
  }

  getStatusLabel(status: VehicleStatus): string {
    switch (status) {
      case VehicleStatus.Available:
        return 'Disponible';
      case VehicleStatus.Rented:
        return 'En renta';
      case VehicleStatus.InMaintenance:
        return 'Mantenimiento';
      default:
        return 'Desconocido';
    }
  }
}
