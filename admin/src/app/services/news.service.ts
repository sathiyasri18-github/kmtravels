import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { PagedResult } from '../models/common.model';
import { NewsForm, NewsItem } from '../models/news.model';

@Injectable({ providedIn: 'root' })
export class NewsService {
  constructor(private api: ApiService) {}

  getAll(page = 1, pageSize = 20): Observable<PagedResult<NewsItem>> {
    return this.api.get<PagedResult<NewsItem>>(`/admin/news?page=${page}&pageSize=${pageSize}`);
  }

  getById(id: number): Observable<NewsItem> {
    return this.api.get<NewsItem>(`/admin/news/${id}`);
  }

  create(form: NewsForm): Observable<unknown> {
    return this.api.post('/admin/news', form);
  }

  update(id: number, form: NewsForm): Observable<unknown> {
    return this.api.put(`/admin/news/${id}`, form);
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/news/${id}`);
  }

  togglePublish(id: number): Observable<boolean> {
    return this.api.patch(`/admin/news/${id}/publish`);
  }
}
