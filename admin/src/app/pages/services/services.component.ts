import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { CheckboxModule } from 'primeng/checkbox';
import { TagModule } from 'primeng/tag';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ServiceManagementService } from '../../services/service-management.service';
import { defaultServiceForm, defaultServiceImage, ServiceForm, ServiceItem } from '../../models/service.model';
import { RichTextEditorComponent } from '../../shared/rich-text-editor/rich-text-editor.component';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';

@Component({
  selector: 'app-services',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, TextareaModule, CheckboxModule, TagModule,
    ConfirmDialogModule, ToastModule, RichTextEditorComponent, FileUploadComponent
  ],
  templateUrl: './services.component.html',
  styleUrl: './services.component.scss'
})
export class ServicesComponent implements OnInit {
  services: ServiceItem[] = [];
  loading = false;
  dialogVisible = false;
  saving = false;
  editItem: ServiceItem | null = null;
  form: ServiceForm = defaultServiceForm();

  constructor(
    private serviceApi: ServiceManagementService,
    private confirm: ConfirmationService,
    private msg: MessageService
  ) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading = true;
    this.serviceApi.getAll().subscribe({
      next: r => {
        this.services = r ?? [];
        this.loading = false;
      },
      error: () => {
        this.services = [];
        this.loading = false;
        this.msg.add({ severity: 'error', summary: 'Error', detail: 'Failed to load services. Check API connection and login.' });
      }
    });
  }

  openDialog(): void {
    this.editItem = null;
    this.form = defaultServiceForm();
    this.dialogVisible = true;
  }

  edit(item: ServiceItem): void {
    this.editItem = item;
    this.serviceApi.getById(item.id).subscribe(detail => {
      this.form = {
        title: detail.title,
        subtitle: detail.subtitle || '',
        content: detail.content || '',
        coverImageUrl: detail.coverImageUrl || '',
        metaTitle: detail.metaTitle || '',
        metaDescription: detail.metaDescription || '',
        metaKeywords: detail.metaKeywords || '',
        slug: detail.slug,
        sortOrder: detail.sortOrder,
        demandRating: detail.demandRating,
        isPublished: detail.isPublished ?? true,
        isFeatured: detail.isFeatured ?? false,
        images: (detail.images || []).map((img, i) => ({
          title: img.title,
          imageUrl: img.imageUrl,
          caption: img.caption || '',
          sortOrder: img.sortOrder ?? i + 1
        }))
      };
      this.dialogVisible = true;
    });
  }

  autoSlug(): void {
    if (!this.form.slug && this.form.title) {
      this.form.slug = this.form.title.toLowerCase().replace(/[^a-z0-9\s-]/g, '').replace(/\s+/g, '-');
    }
    if (!this.form.metaTitle && this.form.title) {
      this.form.metaTitle = `${this.form.title} | SNL Engineering`;
    }
  }

  addImage(): void {
    this.form.images.push(defaultServiceImage(this.form.images.length + 1));
  }

  removeImage(index: number): void {
    this.form.images.splice(index, 1);
    this.form.images.forEach((img, i) => img.sortOrder = i + 1);
  }

  save(): void {
    this.saving = true;
    const payload = {
      ...this.form,
      images: this.form.images.filter(img => img.imageUrl)
    };
    const op = this.editItem
      ? this.serviceApi.update(this.editItem.id, payload)
      : this.serviceApi.create(payload);

    op.subscribe({
      next: () => {
        this.saving = false;
        this.dialogVisible = false;
        this.load();
        this.msg.add({ severity: 'success', summary: 'Saved', detail: 'Service saved successfully' });
      },
      error: () => {
        this.saving = false;
        this.msg.add({ severity: 'error', summary: 'Error', detail: 'Failed to save service' });
      }
    });
  }

  togglePublish(item: ServiceItem): void {
    this.serviceApi.togglePublish(item.id).subscribe(() => this.load());
  }

  confirmDelete(item: ServiceItem): void {
    this.confirm.confirm({
      message: `Delete "${item.title}"?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.serviceApi.delete(item.id).subscribe(() => {
        this.load();
        this.msg.add({ severity: 'success', summary: 'Deleted' });
      })
    });
  }
}
