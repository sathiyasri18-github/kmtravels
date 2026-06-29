import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) {}

  private headers(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders(token ? { Authorization: `Bearer ${token}` } : {});
  }

  get<T>(url: string): Observable<T> {
    return this.http.get<ApiResponse<T>>(`${environment.apiUrl}${url}`, { headers: this.headers() })
      .pipe(map(r => r.data));
  }

  post<T>(url: string, body: unknown): Observable<T> {
    return this.http.post<ApiResponse<T>>(`${environment.apiUrl}${url}`, body, { headers: this.headers() })
      .pipe(map(r => r.data));
  }

  put<T>(url: string, body: unknown): Observable<T> {
    return this.http.put<ApiResponse<T>>(`${environment.apiUrl}${url}`, body, { headers: this.headers() })
      .pipe(map(r => r.data));
  }

  delete<T>(url: string): Observable<T> {
    return this.http.delete<ApiResponse<T>>(`${environment.apiUrl}${url}`, { headers: this.headers() })
      .pipe(map(r => r.data));
  }

  patch<T>(url: string): Observable<T> {
    return this.http.patch<ApiResponse<T>>(`${environment.apiUrl}${url}`, {}, { headers: this.headers() })
      .pipe(map(r => r.data));
  }

  upload(folder: string, file: File): Observable<string> {
    const formData = new FormData();
    formData.append('file', file);

    let headers = new HttpHeaders();
    const token = localStorage.getItem('token');
    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }

    return this.http.post<ApiResponse<string>>(`${environment.apiUrl}/admin/upload/${folder}`, formData, { headers })
      .pipe(map(r => r.data));
  }
}
