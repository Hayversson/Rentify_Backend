import { VehicleStatus } from './status.model';

export interface Vehicle {
  id: number;
  plate: string;
  model: string;
  year: number;
  status: VehicleStatus;
  vehicleTypeId: number;
  branchId: number;
  createdAt: string;
  updatedAt?: string | null;
}
