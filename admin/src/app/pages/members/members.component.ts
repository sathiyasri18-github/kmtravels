import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TabViewModule } from 'primeng/tabview';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { ApiService } from '../../core/api.service';

interface Member {
  id: number;
  memberType: number;
  status: number;
  fullName: string;
  email: string;
  phone: string;
  district?: string;
  companyName?: string;
  membershipNumber?: string;
  createdAt: string;
}

interface PagedResult<T> { items: T[]; totalCount: number; }

@Component({
  selector: 'app-members',
  standalone: true,
  providers: [MessageService],
  imports: [CommonModule, TableModule, ButtonModule, TagModule, TabViewModule, ToastModule],
  template: `
    <p-toast />
    <h1>Member Registrations</h1>
    <p-tabView (onChange)="onTabChange($event)">
      <p-tabPanel header="Pending">
        <p-table [value]="members" styleClass="p-datatable-sm">
          <ng-template pTemplate="header">
            <tr><th>Name</th><th>Type</th><th>Email</th><th>Phone</th><th>District</th><th>Actions</th></tr>
          </ng-template>
          <ng-template pTemplate="body" let-m>
            <tr>
              <td>{{ m.fullName }}</td>
              <td><p-tag [value]="m.memberType === 1 ? 'Operator' : 'Worker'" /></td>
              <td>{{ m.email }}</td>
              <td>{{ m.phone }}</td>
              <td>{{ m.district }}</td>
              <td>
                <p-button label="Approve" icon="pi pi-check" size="small" (onClick)="approve(m)" />
                <p-button label="Reject" icon="pi pi-times" size="small" severity="danger" (onClick)="reject(m)" />
              </td>
            </tr>
          </ng-template>
        </p-table>
      </p-tabPanel>
      <p-tabPanel header="Approved">
        <p-table [value]="approved" styleClass="p-datatable-sm">
          <ng-template pTemplate="header">
            <tr><th>Name</th><th>Membership #</th><th>Email</th><th>Company</th><th>Date</th></tr>
          </ng-template>
          <ng-template pTemplate="body" let-m>
            <tr>
              <td>{{ m.fullName }}</td>
              <td>{{ m.membershipNumber }}</td>
              <td>{{ m.email }}</td>
              <td>{{ m.companyName }}</td>
              <td>{{ m.createdAt | date:'dd MMM yyyy' }}</td>
            </tr>
          </ng-template>
        </p-table>
      </p-tabPanel>
    </p-tabView>
  `,
  styles: [`h1 { color: #1a3a6b; margin-bottom: 1.5rem; }`]
})
export class MembersComponent implements OnInit {
  members: Member[] = [];
  approved: Member[] = [];

  constructor(private api: ApiService, private msg: MessageService) {}

  ngOnInit(): void { this.loadPending(); this.loadApproved(); }

  loadPending(): void {
    this.api.get<PagedResult<Member>>('/admin/members?status=1').subscribe(r => this.members = r.items);
  }

  loadApproved(): void {
    this.api.get<PagedResult<Member>>('/admin/members?status=2').subscribe(r => this.approved = r.items);
  }

  onTabChange(e: { index: number }): void {
    if (e.index === 0) this.loadPending();
    else this.loadApproved();
  }

  approve(m: Member): void {
    this.api.post(`/admin/members/${m.id}/approve`, {}).subscribe(() => {
      this.loadPending(); this.loadApproved();
      this.msg.add({ severity: 'success', summary: 'Approved', detail: `${m.fullName} approved` });
    });
  }

  reject(m: Member): void {
    this.api.post(`/admin/members/${m.id}/reject`, 'Rejected by admin').subscribe(() => {
      this.loadPending();
      this.msg.add({ severity: 'info', summary: 'Rejected' });
    });
  }
}
