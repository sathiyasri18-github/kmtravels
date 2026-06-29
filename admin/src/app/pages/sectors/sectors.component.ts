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
import { SectorManagementService } from '../../services/sector-management.service';
import { defaultSectorForm, defaultSectorImage, SectorForm, SectorItem } from '../../models/sector.model';
import { RichTextEditorComponent } from '../../shared/rich-text-editor/rich-text-editor.component';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';

@Component({
  selector: 'app-sectors',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, TextareaModule, CheckboxModule, TagModule,
    ConfirmDialogModule, ToastModule, RichTextEditorComponent, FileUploadComponent
  ],
  templateUrl: './sectors.component.html',
  styleUrl: './sectors.component.scss'
})
export class SectorsComponent implements OnInit {
  sectors: SectorItem[] = [];
  loading = false;
  dialogVisible = false;
  saving = false;
  editItem: SectorItem | null = null;
  form: SectorForm = defaultSectorForm();

  constructor(
    private sectorApi: SectorManagementService,
    private confirm: ConfirmationService,
    private msg: MessageService
  ) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading = true;
    this.sectorApi.getAll().subscribe({
      next: r => {
        this.sectors = r ?? [];
        this.loading = false;
      },
      error: () => {
        this.sectors = [];
        this.loading = false;
        this.msg.add({ severity: 'error', summary: 'Error', detail: 'Failed to load sectors. Check API connection and login.' });
      }
    });
  }

  openDialog(): void {
    this.editItem = null;
    this.form = defaultSectorForm();
    this.dialogVisible = true;
  }

  edit(item: SectorItem): void {
    this.editItem = item;
    this.sectorApi.getById(item.id).subscribe(detail => {
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
      this.form.metaTitle = `${this.form.title} Sector | SNL Engineering`;
    }
  }

  addImage(): void {
    this.form.images.push(defaultSectorImage(this.form.images.length + 1));
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
      ? this.sectorApi.update(this.editItem.id, payload)
      : this.sectorApi.create(payload);

    op.subscribe({
      next: () => {
        this.saving = false;
        this.dialogVisible = false;
        this.load();
        this.msg.add({ severity: 'success', summary: 'Saved', detail: 'Sector saved successfully' });
      },
      error: () => {
        this.saving = false;
        this.msg.add({ severity: 'error', summary: 'Error', detail: 'Failed to save sector' });
      }
    });
  }

  togglePublish(item: SectorItem): void {
    this.sectorApi.togglePublish(item.id).subscribe(() => this.load());
  }

  confirmDelete(item: SectorItem): void {
    this.confirm.confirm({
      message: `Delete "${item.title}"?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.sectorApi.delete(item.id).subscribe(() => {
        this.load();
        this.msg.add({ severity: 'success', summary: 'Deleted' });
      })
    });
  }
}
