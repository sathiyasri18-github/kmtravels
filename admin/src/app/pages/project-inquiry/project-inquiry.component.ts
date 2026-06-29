import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { TagModule } from 'primeng/tag';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ProjectInquiryService } from '../../services/project-inquiry.service';
import {
  defaultProjectInquiryForm,
  PROJECT_INQUIRY_STATUS,
  ProjectInquiry,
  ProjectInquiryForm,
} from '../../models/project-inquiry.model';

@Component({
  selector: 'app-project-inquiry',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, TextareaModule, SelectModule, TagModule,
    ConfirmDialogModule, ToastModule,
  ],
  templateUrl: './project-inquiry.component.html',
  styleUrl: './project-inquiry.component.scss'
})
export class ProjectInquiryComponent implements OnInit {
  inquiries: ProjectInquiry[] = [];
  selected: ProjectInquiry | null = null;
  editItem: ProjectInquiry | null = null;
  form: ProjectInquiryForm = defaultProjectInquiryForm();
  viewDialog = false;
  dialogVisible = false;
  saving = false;
  statusOptions = PROJECT_INQUIRY_STATUS;

  constructor(
    private service: ProjectInquiryService,
    private confirm: ConfirmationService,
    private msg: MessageService,
  ) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.service.getAll(1, 100).subscribe(r => this.inquiries = r.items);
  }

  view(item: ProjectInquiry): void {
    this.selected = item;
    this.viewDialog = true;
  }

  openDialog(): void {
    this.editItem = null;
    this.form = defaultProjectInquiryForm();
    this.dialogVisible = true;
  }

  edit(item: ProjectInquiry): void {
    this.editItem = item;
    this.form = {
      fullName: item.fullName,
      email: item.email,
      phone: item.phone,
      companyName: item.companyName || '',
      city: item.city || '',
      state: item.state || '',
      projectType: item.projectType || '',
      projectDetails: item.projectDetails || '',
      status: item.status,
      notes: item.notes || '',
    };
    this.dialogVisible = true;
  }

  save(): void {
    this.saving = true;
    const op = this.editItem
      ? this.service.update(this.editItem.id, this.form)
      : this.service.create(this.form);

    op.subscribe({
      next: () => {
        this.saving = false;
        this.dialogVisible = false;
        this.load();
        this.msg.add({ severity: 'success', summary: 'Saved' });
      },
      error: () => { this.saving = false; },
    });
  }

  confirmDelete(item: ProjectInquiry): void {
    this.confirm.confirm({
      message: `Delete inquiry from "${item.fullName}"?`,
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.service.delete(item.id).subscribe(() => {
        this.load();
        this.msg.add({ severity: 'success', summary: 'Deleted' });
      }),
    });
  }

  getStatusLabel(status: number): string {
    return this.statusOptions.find(s => s.value === status)?.label ?? 'Unknown';
  }

  getStatusSeverity(status: number): 'info' | 'success' | 'secondary' | 'warn' {
    if (status === 1) return 'warn';
    if (status === 2) return 'success';
    return 'secondary';
  }
}
