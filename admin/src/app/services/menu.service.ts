import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { MenuForm, MenuItem } from '../models/menu.model';

@Injectable({ providedIn: 'root' })
export class MenuService {
  constructor(private api: ApiService) {}

  getAll(): Observable<MenuItem[]> {
    return this.api.get<MenuItem[]>('/admin/menu');
  }

  create(form: MenuForm): Observable<unknown> {
    return this.api.post('/admin/menu', this.toDto(form));
  }

  update(id: number, form: MenuForm): Observable<unknown> {
    return this.api.put(`/admin/menu/${id}`, { id, children: [], ...this.toDto(form) });
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/menu/${id}`);
  }

  private toDto(form: MenuForm) {
    return {
      title: form.title,
      url: form.url || null,
      parentId: form.parentId,
      sortOrder: form.sortOrder,
      openInNewTab: form.openInNewTab,
    };
  }
}
