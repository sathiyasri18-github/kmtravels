import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { PagedProjectInquiries, ProjectInquiry, ProjectInquiryForm } from '../models/project-inquiry.model';

@Injectable({ providedIn: 'root' })
export class ProjectInquiryService {
  constructor(private api: ApiService) {}

  getAll(page = 1, pageSize = 20, status?: number): Observable<PagedProjectInquiries> {
    const statusQuery = status != null ? `&status=${status}` : '';
    return this.api.get<PagedProjectInquiries>(`/admin/project-inquiries?page=${page}&pageSize=${pageSize}${statusQuery}`);
  }

  getById(id: number): Observable<ProjectInquiry> {
    return this.api.get<ProjectInquiry>(`/admin/project-inquiries/${id}`);
  }

  create(form: ProjectInquiryForm): Observable<ProjectInquiry> {
    return this.api.post<ProjectInquiry>('/admin/project-inquiries', form);
  }

  update(id: number, form: ProjectInquiryForm): Observable<ProjectInquiry> {
    return this.api.put<ProjectInquiry>(`/admin/project-inquiries/${id}`, form);
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/project-inquiries/${id}`);
  }
}
