import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Customer } from '../models/customer.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class CustomersService {
  private api = inject(ApiService);

  getCustomers(): Observable<Customer[]> {
    return this.api.get<Customer[]>('Customer');
  }

  getCustomerById(id: number): Observable<Customer> {
    return this.api.get<Customer>(`Customer/${id}`);
  }
}
