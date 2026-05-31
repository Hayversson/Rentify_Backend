import { Rental } from './rental.model';

export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  licenseNumber: string;
  licenseExpirationDate: string;
  rentals?: Rental[];
}
