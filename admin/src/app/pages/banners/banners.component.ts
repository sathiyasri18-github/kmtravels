import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BannerService } from '../../services/banner.service';
import { BannerForm, BannerSlide, defaultBannerForm } from '../../models/banner.model';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';

@Component({
  selector: 'app-banners',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, InputNumberModule, ConfirmDialogModule, ToastModule, FileUploadComponent
  ],
  templateUrl: './banners.component.html',
  styleUrl: './banners.component.scss'
})
export class BannersComponent implements OnInit {
  banners: BannerSlide[] = [];
  dialogVisible = false;
  saving = false;
  editItem: BannerSlide | null = null;
  form: BannerForm = defaultBannerForm();

  constructor(private service: BannerService, private confirm: ConfirmationService, private msg: MessageService) {}

  ngOnInit(): void { this.load(); }

  load(): void { this.service.getAll().subscribe(b => this.banners = b); }

  openDialog(): void { this.editItem = null; this.form = defaultBannerForm(); this.dialogVisible = true; }

  edit(item: BannerSlide): void {
    this.editItem = item;
    this.form = { title: item.title, subtitle: item.subtitle || '', imageUrl: item.imageUrl, linkUrl: item.linkUrl || '', sortOrder: item.sortOrder };
    this.dialogVisible = true;
  }

  save(): void {
    this.saving = true;
    const op = this.editItem ? this.service.update(this.editItem.id, this.form) : this.service.create(this.form);
    op.subscribe({
      next: () => { this.saving = false; this.dialogVisible = false; this.load(); this.msg.add({ severity: 'success', summary: 'Saved' }); },
      error: () => { this.saving = false; }
    });
  }

  confirmDelete(item: BannerSlide): void {
    this.confirm.confirm({
      message: `Delete "${item.title}"?`, header: 'Confirm', icon: 'pi pi-exclamation-triangle',
      accept: () => this.service.delete(item.id).subscribe(() => { this.load(); this.msg.add({ severity: 'success', summary: 'Deleted' }); })
    });
  }
}
