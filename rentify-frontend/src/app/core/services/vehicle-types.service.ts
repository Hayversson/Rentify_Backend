import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { VehicleType } from '../models/vehicle-type.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class VehicleTypesService {
  private api = inject(ApiService);

  getVehicleTypes(): Observable<VehicleType[]> {
    return this.api.get<VehicleType[]>('VehicleType');
  }

  getVehicleTypeById(id: number): Observable<VehicleType> {
    return this.api.get<VehicleType>(`VehicleType/${id}`);
  }
}
