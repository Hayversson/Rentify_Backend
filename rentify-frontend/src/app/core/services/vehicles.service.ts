import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Vehicle } from '../models/vehicle.model';
import { VehicleStatus } from '../models/status.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class VehiclesService {
  private api = inject(ApiService);

  getVehicles(): Observable<Vehicle[]> {
    return this.api.get<Vehicle[]>('Vehicle');
  }

  getVehicleById(id: number): Observable<Vehicle> {
    return this.api.get<Vehicle>(`Vehicle/${id}`);
  }

  getVehiclesByStatus(status: VehicleStatus): Observable<Vehicle[]> {
    return this.api.get<Vehicle[]>(`Vehicle/status/${status}`);
  }
}
