import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Rental, RentalRequest } from '../models/rental.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class RentalsService {
  private api = inject(ApiService);

  getRentals(): Observable<Rental[]> {
    return this.api.get<Rental[]>('Rental');
  }

  getRentalsByCustomer(customerId: number): Observable<Rental[]> {
    return this.api.get<Rental[]>(`Rental/customer/${customerId}`);
  }

  createRental(payload: RentalRequest, paymentMethod: string): Observable<Rental> {
    return this.api.post<Rental>(`Rental?paymentMethod=${encodeURIComponent(paymentMethod)}`, payload);
  }
}
