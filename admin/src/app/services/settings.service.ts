import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { SiteSettings } from '../models/settings.model';

@Injectable({ providedIn: 'root' })
export class SettingsService {
  constructor(private api: ApiService) {}

  get(): Observable<SiteSettings> {
    return this.api.get<SiteSettings>('/admin/settings');
  }

  update(settings: SiteSettings): Observable<boolean> {
    return this.api.put('/admin/settings', settings);
  }
}
