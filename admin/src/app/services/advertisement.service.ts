import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { Advertisement, AdvertisementForm } from '../models/advertisement.model';

@Injectable({ providedIn: 'root' })
export class AdvertisementService {
  constructor(private api: ApiService) {}

  getAll(): Observable<Advertisement[]> {
    return this.api.get<Advertisement[]>('/admin/advertisements');
  }

  create(form: AdvertisementForm): Observable<unknown> {
    return this.api.post('/admin/advertisements', this.toDto(form));
  }

  update(id: number, form: AdvertisementForm): Observable<unknown> {
    return this.api.put(`/admin/advertisements/${id}`, { id, ...this.toDto(form) });
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/advertisements/${id}`);
  }

  private toDto(form: AdvertisementForm) {
    return {
      title: form.title,
      imageUrl: form.imageUrl,
      linkUrl: form.linkUrl || null,
      position: form.position,
      startDate: form.startDate,
      endDate: form.endDate,
      sortOrder: form.sortOrder,
    };
  }
}
