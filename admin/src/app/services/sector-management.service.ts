import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { SectorForm, SectorItem } from '../models/sector.model';

@Injectable({ providedIn: 'root' })
export class SectorManagementService {
  constructor(private api: ApiService) {}

  getAll(): Observable<SectorItem[]> {
    return this.api.get<SectorItem[]>('/admin/sectors');
  }

  getById(id: number): Observable<SectorItem> {
    return this.api.get<SectorItem>(`/admin/sectors/${id}`);
  }

  create(form: SectorForm): Observable<unknown> {
    return this.api.post('/admin/sectors', form);
  }

  update(id: number, form: SectorForm): Observable<unknown> {
    return this.api.put(`/admin/sectors/${id}`, form);
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/sectors/${id}`);
  }

  togglePublish(id: number): Observable<boolean> {
    return this.api.patch(`/admin/sectors/${id}/publish`);
  }
}
