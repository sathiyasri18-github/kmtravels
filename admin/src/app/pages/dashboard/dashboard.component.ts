import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ApiService } from '../../core/api.service';

interface DashboardStats {
  totalVisitors: number;
  totalPageViews: number;
  totalNews: number;
  totalProjectInquiries: number;
  pendingProjectInquiries: number;
  totalInquiries: number;
  unreadInquiries: number;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, CardModule],
  template: `
    <h1>Dashboard</h1>
    <p class="subtitle">SnlEngineering CMS Overview</p>
    <div class="stats-grid">
      @for (stat of stats; track stat.label) {
        <p-card>
          <div class="stat-card">
            <i [class]="stat.icon"></i>
            <div>
              <span class="stat-value">{{ stat.value }}</span>
              <span class="stat-label">{{ stat.label }}</span>
            </div>
          </div>
        </p-card>
      }
    </div>
  `,
  styles: [`
    h1 { margin: 0 0 0.25rem; color: #1a3a6b; }
    .subtitle { color: #475569; margin-bottom: 2rem; }
    .stats-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(220px, 1fr)); gap: 1.5rem; }
    .stat-card { display: flex; align-items: center; gap: 1rem; }
    .stat-card i { font-size: 2rem; color: #1a3a6b; }
    .stat-value { display: block; font-size: 1.75rem; font-weight: 700; color: #1a3a6b; }
    .stat-label { font-size: 0.85rem; color: #475569; }
  `]
})
export class DashboardComponent implements OnInit {
  stats: { label: string; value: number; icon: string }[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.get<DashboardStats>('/admin/dashboard').subscribe(data => {
      this.stats = [
        { label: 'Total Visitors', value: data.totalVisitors, icon: 'pi pi-users' },
        { label: 'Page Views', value: data.totalPageViews, icon: 'pi pi-eye' },
        { label: 'News Articles', value: data.totalNews, icon: 'pi pi-file' },
        { label: 'Project Inquiries', value: data.totalProjectInquiries, icon: 'pi pi-inbox' },
        { label: 'Pending Inquiries', value: data.pendingProjectInquiries, icon: 'pi pi-clock' },
        { label: 'Contact Inquiries', value: data.totalInquiries, icon: 'pi pi-envelope' },
        { label: 'Unread Inquiries', value: data.unreadInquiries, icon: 'pi pi-inbox' },
      ];
    });
  }
}
