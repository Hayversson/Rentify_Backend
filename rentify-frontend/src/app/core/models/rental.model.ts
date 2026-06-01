import { RentalStatus } from './status.model';

export interface Rental {
  id: number;
  startDate: string;
  endDate: string;
  totalCost: number;
  status: RentalStatus;
  customerId: number;
  vehicleId: number;
  pickupBranchId: number;
  returnBranchId: number;
}

export interface RentalRequest {
  startDate: string;
  endDate: string;
  customerId: number;
  vehicleId: number;
  pickupBranchId: number;
  returnBranchId: number;
}
