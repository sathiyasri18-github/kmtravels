import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { Publication, PublicationForm } from '../models/publication.model';

@Injectable({ providedIn: 'root' })
export class PublicationService {
  constructor(private api: ApiService) {}

  getAll(): Observable<Publication[]> {
    return this.api.get<Publication[]>('/admin/publications');
  }

  create(form: PublicationForm): Observable<unknown> {
    return this.api.post('/admin/publications', this.toRequest(form));
  }

  update(id: number, form: PublicationForm): Observable<unknown> {
    return this.api.put(`/admin/publications/${id}`, this.toRequest(form));
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/publications/${id}`);
  }

  private toRequest(form: PublicationForm) {
    return {
      title: form.title,
      type: form.type,
      edition: form.edition || null,
      description: form.description || null,
      pdfUrl: form.pdfUrl,
      coverImageUrl: form.coverImageUrl || null,
      publishedDate: form.publishedDate || null,
      isPublished: form.isPublished,
    };
  }
}
