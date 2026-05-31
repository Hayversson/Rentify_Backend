import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Branch } from '../models/branch.model';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class BranchesService {
  private api = inject(ApiService);

  getBranches(): Observable<Branch[]> {
    return this.api.get<Branch[]>('Branch');
  }

  getActiveBranches(): Observable<Branch[]> {
    return this.api.get<Branch[]>('Branch/active');
  }

  getBranchById(id: number): Observable<Branch> {
    return this.api.get<Branch>(`Branch/${id}`);
  }
}
