import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { EventForm, EventItem } from '../models/event.model';

@Injectable({ providedIn: 'root' })
export class EventService {
  constructor(private api: ApiService) {}

  getAll(): Observable<EventItem[]> {
    return this.api.get<EventItem[]>('/admin/events');
  }

  create(form: EventForm): Observable<unknown> {
    return this.api.post('/admin/events', this.toDto(form));
  }

  update(id: number, form: EventForm): Observable<unknown> {
    return this.api.put(`/admin/events/${id}`, { id, ...this.toDto(form) });
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/events/${id}`);
  }

  private toDto(form: EventForm) {
    return {
      title: form.title,
      description: form.description || null,
      location: form.location || null,
      startDate: form.startDate,
      endDate: form.endDate || null,
      imageUrl: form.imageUrl || null,
      isPublished: form.isPublished,
    };
  }
}
