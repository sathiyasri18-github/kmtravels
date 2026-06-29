import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { Observable, tap } from 'rxjs';
import { map } from 'rxjs/operators';

export interface LoginResponse {
  token: string;
  email: string;
  fullName: string;
  roles: string[];
  expiresAt: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<{ success: boolean; data: LoginResponse }>(`${environment.apiUrl}/auth/login`, { email, password })
      .pipe(
        map(r => r.data),
        tap(data => {
          localStorage.setItem('token', data.token);
          localStorage.setItem('user', JSON.stringify(data));
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getUser(): LoginResponse | null {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }
}
