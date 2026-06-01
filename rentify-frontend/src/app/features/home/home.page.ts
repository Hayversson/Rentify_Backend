import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Branch } from '../../core/models/branch.model';
import { VehicleStatus } from '../../core/models/status.model';
import { VehicleType } from '../../core/models/vehicle-type.model';
import { Vehicle } from '../../core/models/vehicle.model';
import { BranchesService } from '../../core/services/branches.service';
import { VehicleTypesService } from '../../core/services/vehicle-types.service';
import { VehiclesService } from '../../core/services/vehicles.service';

interface FeaturedVehicle {
  id: number;
  title: string;
  subtitle: string;
  pricePerDay: number | null;
  status: VehicleStatus;
}

interface HomeStats {
  totalVehicles: number;
  availableVehicles: number;
  totalBranches: number;
  totalTypes: number;
}

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.page.html',
  styleUrl: './home.page.scss'
})
export class HomePageComponent implements OnInit {
  protected readonly featuredVehicles = signal<FeaturedVehicle[]>([]);
  protected readonly branches = signal<Branch[]>([]);
  protected readonly vehicleTypes = signal<VehicleType[]>([]);
  protected readonly stats = signal<HomeStats>({
    totalVehicles: 0,
    availableVehicles: 0,
    totalBranches: 0,
    totalTypes: 0
  });
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal<string | null>(null);
  protected readonly allVehicles = signal<Vehicle[]>([]);
  protected readonly selectedBranchId = signal<number | null>(null);

  constructor(
    private vehiclesService: VehiclesService,
    private vehicleTypesService: VehicleTypesService,
    private branchesService: BranchesService
  ) {}

  ngOnInit(): void {
    forkJoin({
      vehicles: this.vehiclesService.getVehicles(),
      types: this.vehicleTypesService.getVehicleTypes(),
      branches: this.branchesService.getActiveBranches()
    }).subscribe({
      next: ({ vehicles, types, branches }) => {
        const typeMap = new Map(types.map((type) => [type.id, type]));

        this.vehicleTypes.set(types);
        this.branches.set(branches);
        this.allVehicles.set(vehicles);
        this.selectedBranchId.set(branches.length > 0 ? branches[0].id : null);
        this.featuredVehicles.set(this.buildFeaturedVehicles(vehicles, typeMap));
        this.stats.set(this.buildStats(vehicles, types, branches));
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('No se pudieron cargar los datos del backend.');
        this.isLoading.set(false);
      }
    });
  }

  private buildFeaturedVehicles(
    vehicles: Vehicle[],
    typeMap: Map<number, VehicleType>
  ): FeaturedVehicle[] {
    return vehicles.slice(0, 3).map((vehicle) => {
      const type = typeMap.get(vehicle.vehicleTypeId);
        return {
          id: vehicle.id,
          title: vehicle.model,
          subtitle: `${vehicle.plate} · ${type?.name ?? 'Tipo no disponible'}`,
          pricePerDay: type?.pricePerDay ?? null,
          status: vehicle.status
        };
      });
  }

  private buildStats(
    vehicles: Vehicle[],
    types: VehicleType[],
    branches: Branch[]
  ): HomeStats {
    return {
      totalVehicles: vehicles.length,
      availableVehicles: vehicles.filter((vehicle) => vehicle.status === VehicleStatus.Available)
        .length,
      totalBranches: branches.length,
      totalTypes: types.length
    };
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

  selectBranch(branchId: number): void {
    this.selectedBranchId.set(branchId);
  }

  getVehiclesForBranch(): Vehicle[] {
    const branchId = this.selectedBranchId();
    if (!branchId) return [];
    return this.allVehicles().filter((v) => v.branchId === branchId);
  }
}
