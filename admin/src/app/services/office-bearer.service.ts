import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { OfficeBearer, OfficeBearerForm } from '../models/office-bearer.model';

@Injectable({ providedIn: 'root' })
export class OfficeBearerService {
  constructor(private api: ApiService) {}

  getAll(): Observable<OfficeBearer[]> {
    return this.api.get<OfficeBearer[]>('/admin/office-bearers');
  }

  create(form: OfficeBearerForm): Observable<unknown> {
    return this.api.post('/admin/office-bearers', this.toRequest(form));
  }

  update(id: number, form: OfficeBearerForm): Observable<unknown> {
    return this.api.put(`/admin/office-bearers/${id}`, this.toRequest(form));
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/office-bearers/${id}`);
  }

  private toRequest(form: OfficeBearerForm) {
    return {
      name: form.name,
      role: form.role,
      designation: form.designation || null,
      district: form.district || null,
      phone: form.phone || null,
      email: form.email || null,
      photoUrl: form.photoUrl || null,
      sortOrder: form.sortOrder,
      bio: form.bio || null,
    };
  }
}
