import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { ContactInquiry, PagedInquiries } from '../models/contact.model';

@Injectable({ providedIn: 'root' })
export class ContactService {
  constructor(private api: ApiService) {}

  getAll(page = 1, pageSize = 20): Observable<PagedInquiries> {
    return this.api.get<PagedInquiries>(`/admin/contact?page=${page}&pageSize=${pageSize}`);
  }

  markAsRead(id: number): Observable<boolean> {
    return this.api.patch(`/admin/contact/${id}/read`);
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/contact/${id}`);
  }
}
