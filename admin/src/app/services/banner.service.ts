import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { BannerForm, BannerSlide } from '../models/banner.model';

@Injectable({ providedIn: 'root' })
export class BannerService {
  constructor(private api: ApiService) {}

  getAll(): Observable<BannerSlide[]> {
    return this.api.get<BannerSlide[]>('/admin/banners');
  }

  create(form: BannerForm): Observable<unknown> {
    return this.api.post('/admin/banners', this.toDto(form));
  }

  update(id: number, form: BannerForm): Observable<unknown> {
    return this.api.put(`/admin/banners/${id}`, { id, ...this.toDto(form) });
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/banners/${id}`);
  }

  private toDto(form: BannerForm) {
    return {
      title: form.title,
      subtitle: form.subtitle || null,
      imageUrl: form.imageUrl,
      linkUrl: form.linkUrl || null,
      sortOrder: form.sortOrder,
    };
  }
}
