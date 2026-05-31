import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Branch } from '../../core/models/branch.model';
import { Vehicle } from '../../core/models/vehicle.model';
import { VehicleType } from '../../core/models/vehicle-type.model';
import { VehicleStatus } from '../../core/models/status.model';
import { BranchesService } from '../../core/services/branches.service';
import { VehicleTypesService } from '../../core/services/vehicle-types.service';
import { VehiclesService } from '../../core/services/vehicles.service';

interface VehicleView {
  id: number;
  model: string;
  plate: string;
  year: number;
  status: VehicleStatus;
  vehicleTypeId: number;
  branchId: number;
  typeName: string;
  pricePerDay: number | null;
  branchName: string;
}

@Component({
  selector: 'app-vehicles-page',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './vehicles.page.html',
  styleUrl: './vehicles.page.scss'
})
export class VehiclesPageComponent implements OnInit {
  protected readonly vehicles = signal<VehicleView[]>([]);
  protected readonly branches = signal<Branch[]>([]);
  protected readonly vehicleTypes = signal<VehicleType[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal<string | null>(null);

  selectedTypeId: number | 'all' = 'all';
  selectedBranchId: number | 'all' = 'all';
  selectedStatus: VehicleStatus | 'all' = 'all';

  protected readonly statusOptions = [
    { label: 'Disponibles', value: VehicleStatus.Available },
    { label: 'En renta', value: VehicleStatus.Rented },
    { label: 'En mantenimiento', value: VehicleStatus.InMaintenance }
  ];

  constructor(
    private vehiclesService: VehiclesService,
    private vehicleTypesService: VehicleTypesService,
    private branchesService: BranchesService
  ) {}

  ngOnInit(): void {
    forkJoin({
      vehicles: this.vehiclesService.getVehicles(),
      types: this.vehicleTypesService.getVehicleTypes(),
      branches: this.branchesService.getBranches()
    }).subscribe({
      next: ({ vehicles, types, branches }) => {
        const typeMap = new Map(types.map((type) => [type.id, type]));
        const branchMap = new Map(branches.map((branch) => [branch.id, branch]));

        this.vehicleTypes.set(types);
        this.branches.set(branches);
        this.vehicles.set(
          vehicles.map((vehicle) => ({
            id: vehicle.id,
            model: vehicle.model,
            plate: vehicle.plate,
            year: vehicle.year,
            status: vehicle.status,
            vehicleTypeId: vehicle.vehicleTypeId,
            branchId: vehicle.branchId,
            typeName: typeMap.get(vehicle.vehicleTypeId)?.name ?? 'Tipo no disponible',
            pricePerDay: typeMap.get(vehicle.vehicleTypeId)?.pricePerDay ?? null,
            branchName: branchMap.get(vehicle.branchId)?.name ?? 'Sucursal no disponible'
          }))
        );
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('No se pudo cargar el catálogo desde el backend.');
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

  get filteredVehicles(): VehicleView[] {
    return this.vehicles().filter((vehicle) => {
      const matchType =
        this.selectedTypeId === 'all' || vehicle.vehicleTypeId === this.selectedTypeId;
      const matchBranch =
        this.selectedBranchId === 'all' || vehicle.branchId === this.selectedBranchId;
      const matchStatus = this.selectedStatus === 'all' || vehicle.status === this.selectedStatus;
      return matchType && matchBranch && matchStatus;
    });
  }

}
