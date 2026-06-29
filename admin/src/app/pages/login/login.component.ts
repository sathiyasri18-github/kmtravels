import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { AuthService } from '../../core/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, CardModule, InputTextModule, PasswordModule, ButtonModule, MessageModule],
  template: `
    <div class="login-page">
      <p-card header="SNL Engineering Admin Panel" subheader="Sign in to manage content">
        @if (error) { <p-message severity="error" [text]="error" styleClass="mb-3 w-full" /> }
        <div class="field">
          <label>Email</label>
          <input pInputText [(ngModel)]="email" class="w-full" type="email" />
        </div>
        <div class="field">
          <label>Password</label>
          <p-password [(ngModel)]="password" [feedback]="false" styleClass="w-full" inputStyleClass="w-full" />
        </div>
        <p-button label="Sign In" icon="pi pi-sign-in" (onClick)="login()" [loading]="loading" styleClass="w-full mt-3" />
        <p class="hint mt-3">Default: admin&#64;snlengineering.com / Admin&#64;123</p>
      </p-card>
    </div>
  `,
  styles: [`
    .login-page { min-height: 100vh; display: flex; align-items: center; justify-content: center; background: linear-gradient(135deg, #1a3a6b, #2d5aa0); padding: 1rem; }
    .login-page p-card { width: 100%; max-width: 420px; }
    .field { margin-bottom: 1rem; }
    .field label { display: block; margin-bottom: 0.4rem; font-weight: 600; }
    .hint { font-size: 0.85rem; color: #64748b; text-align: center; }
  `]
})
export class LoginComponent {
  email = 'admin@snlengineering.com';
  password = '';
  loading = false;
  error = '';

  constructor(private auth: AuthService, private router: Router) {
    if (auth.isLoggedIn()) router.navigate(['/dashboard']);
  }

  login(): void {
    this.loading = true;
    this.error = '';
    this.auth.login(this.email, this.password).subscribe({
      next: () => { this.loading = false; this.router.navigate(['/dashboard']); },
      error: () => { this.loading = false; this.error = 'Invalid email or password'; }
    });
  }
}
