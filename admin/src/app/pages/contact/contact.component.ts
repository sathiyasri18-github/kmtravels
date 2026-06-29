import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { TagModule } from 'primeng/tag';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ContactService } from '../../services/contact.service';
import { ContactInquiry } from '../../models/contact.model';

@Component({
  selector: 'app-contact',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [CommonModule, TableModule, ButtonModule, DialogModule, TagModule, ConfirmDialogModule, ToastModule],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.scss'
})
export class ContactComponent implements OnInit {
  inquiries: ContactInquiry[] = [];
  viewDialog = false;
  selected: ContactInquiry | null = null;

  constructor(private service: ContactService, private confirm: ConfirmationService, private msg: MessageService) {}

  ngOnInit(): void { this.load(); }

  load(): void { this.service.getAll().subscribe(r => this.inquiries = r.items); }

  view(item: ContactInquiry): void {
    this.selected = item;
    this.viewDialog = true;
    if (!item.isRead) this.markRead(item);
  }

  markRead(item: ContactInquiry): void {
    this.service.markAsRead(item.id).subscribe(() => {
      item.isRead = true;
      this.msg.add({ severity: 'success', summary: 'Marked as read' });
    });
  }

  confirmDelete(item: ContactInquiry): void {
    this.confirm.confirm({
      message: `Delete inquiry from "${item.name}"?`, header: 'Confirm', icon: 'pi pi-exclamation-triangle',
      accept: () => this.service.delete(item.id).subscribe(() => { this.load(); this.msg.add({ severity: 'success', summary: 'Deleted' }); })
    });
  }
}
