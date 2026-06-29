import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { ServiceForm, ServiceItem } from '../models/service.model';

@Injectable({ providedIn: 'root' })
export class ServiceManagementService {
  constructor(private api: ApiService) {}

  getAll(): Observable<ServiceItem[]> {
    return this.api.get<ServiceItem[]>('/admin/services');
  }

  getById(id: number): Observable<ServiceItem> {
    return this.api.get<ServiceItem>(`/admin/services/${id}`);
  }

  create(form: ServiceForm): Observable<unknown> {
    return this.api.post('/admin/services', form);
  }

  update(id: number, form: ServiceForm): Observable<unknown> {
    return this.api.put(`/admin/services/${id}`, form);
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/services/${id}`);
  }

  togglePublish(id: number): Observable<boolean> {
    return this.api.patch(`/admin/services/${id}/publish`);
  }
}
