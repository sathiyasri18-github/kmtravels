import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { DynamicPage, PageForm } from '../models/page.model';

@Injectable({ providedIn: 'root' })
export class PageService {
  constructor(private api: ApiService) {}

  getAll(): Observable<DynamicPage[]> {
    return this.api.get<DynamicPage[]>('/admin/pages');
  }

  create(form: PageForm): Observable<unknown> {
    return this.api.post('/admin/pages', this.toDto(form));
  }

  update(id: number, form: PageForm): Observable<unknown> {
    return this.api.put(`/admin/pages/${id}`, { id, ...this.toDto(form) });
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/pages/${id}`);
  }

  private toDto(form: PageForm) {
    return {
      title: form.title,
      slug: form.slug,
      content: form.content,
      metaDescription: form.metaDescription || null,
      isPublished: form.isPublished,
    };
  }
}
