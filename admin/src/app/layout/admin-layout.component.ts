import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MenubarModule } from 'primeng/menubar';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../core/auth.service';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, MenubarModule, ButtonModule],
  template: `
    <div class="admin-layout">
      <aside class="sidebar">
        <div class="sidebar-header">
          <h2>SNL Engineering</h2>
          <small>India Admin</small>
        </div>
        <nav>
          @for (item of menuItems; track item.route) {
            <a [routerLink]="item.route" routerLinkActive="active">
              <i [class]="item.icon"></i> {{ item.label }}
            </a>
          }
        </nav>
        <div class="sidebar-footer">
          <span>{{ user?.fullName }}</span>
          <p-button label="Logout" icon="pi pi-sign-out" severity="secondary" size="small" (onClick)="logout()" />
        </div>
      </aside>
      <main class="content">
        <router-outlet />
      </main>
    </div>
  `,
  styles: [`
    .admin-layout { display: flex; min-height: 100vh; }
    .sidebar { width: 260px; background: #1a3a6b; color: white; display: flex; flex-direction: column; flex-shrink: 0; }
    .sidebar-header { padding: 1.5rem; border-bottom: 1px solid rgba(255,255,255,0.1); }
    .sidebar-header h2 { margin: 0; color: #e8a317; }
    .sidebar-header small { opacity: 1; color: #cbd5e1; }
    nav { flex: 1; padding: 1rem 0; }
    nav a { display: flex; align-items: center; gap: 0.75rem; padding: 0.75rem 1.5rem; color: #e2e8f0; text-decoration: none; font-size: 0.9rem; }
    nav a:hover, nav a.active { background: rgba(255,255,255,0.1); color: white; }
    .sidebar-footer { padding: 1rem 1.5rem; border-top: 1px solid rgba(255,255,255,0.1); display: flex; flex-direction: column; gap: 0.5rem; font-size: 0.85rem; }
    .content { flex: 1; background: #f1f5f9; padding: 2rem; overflow-y: auto; }
    @media (max-width: 768px) { .sidebar { width: 200px; } .content { padding: 1rem; } }
  `]
})
export class AdminLayoutComponent {
  user: ReturnType<AuthService['getUser']>;

  menuItems = [
    { label: 'Dashboard', route: '/dashboard', icon: 'pi pi-home' },
    { label: 'News', route: '/news', icon: 'pi pi-file' },
    { label: 'Services', route: '/services', icon: 'pi pi-briefcase' },
    { label: 'Projects', route: '/gallery', icon: 'pi pi-images' },
    { label: 'Videos', route: '/videos', icon: 'pi pi-video' },
    { label: 'Sectors', route: '/sectors', icon: 'pi pi-sitemap' },
    { label: 'Project Inquiry', route: '/project-inquiry', icon: 'pi pi-inbox' },
    { label: 'Leadership', route: '/office-bearers', icon: 'pi pi-id-card' },
    { label: 'Advertisements', route: '/advertisements', icon: 'pi pi-megaphone' },
    { label: 'Banners', route: '/banners', icon: 'pi pi-image' },
    { label: 'Events', route: '/events', icon: 'pi pi-calendar' },
    { label: 'Contact Inquiries', route: '/contact', icon: 'pi pi-envelope' },
    { label: 'Menu', route: '/menu', icon: 'pi pi-bars' },
    { label: 'Pages', route: '/pages', icon: 'pi pi-file-edit' },
    { label: 'Settings', route: '/settings', icon: 'pi pi-cog' },
  ];

  constructor(public auth: AuthService, private router: Router) {
    this.user = auth.getUser();
  }

  logout(): void { this.auth.logout(); }
}
