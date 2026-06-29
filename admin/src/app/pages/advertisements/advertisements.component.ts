import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelectModule } from 'primeng/select';
import { TagModule } from 'primeng/tag';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AdvertisementService } from '../../services/advertisement.service';
import { Advertisement, AdvertisementForm, defaultAdvertisementForm } from '../../models/advertisement.model';
import { AdvertisementPositions, labelFor } from '../../models/enums.model';
import { toDateInput } from '../../models/common.model';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';

@Component({
  selector: 'app-advertisements',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, InputNumberModule, SelectModule, TagModule,
    ConfirmDialogModule, ToastModule, FileUploadComponent
  ],
  templateUrl: './advertisements.component.html',
  styleUrl: './advertisements.component.scss'
})
export class AdvertisementsComponent implements OnInit {
  ads: Advertisement[] = [];
  dialogVisible = false;
  saving = false;
  editItem: Advertisement | null = null;
  form: AdvertisementForm = defaultAdvertisementForm();
  positionOptions = AdvertisementPositions;

  constructor(private service: AdvertisementService, private confirm: ConfirmationService, private msg: MessageService) {}

  ngOnInit(): void { this.load(); }

  getPositionLabel(v: number): string { return labelFor(AdvertisementPositions, v); }

  load(): void { this.service.getAll().subscribe(a => this.ads = a); }

  openDialog(): void { this.editItem = null; this.form = defaultAdvertisementForm(); this.dialogVisible = true; }

  edit(item: Advertisement): void {
    this.editItem = item;
    this.form = {
      title: item.title, imageUrl: item.imageUrl, linkUrl: item.linkUrl || '',
      position: item.position, startDate: toDateInput(item.startDate),
      endDate: toDateInput(item.endDate), sortOrder: item.sortOrder,
    };
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

  confirmDelete(item: Advertisement): void {
    this.confirm.confirm({
      message: `Delete "${item.title}"?`, header: 'Confirm', icon: 'pi pi-exclamation-triangle',
      accept: () => this.service.delete(item.id).subscribe(() => { this.load(); this.msg.add({ severity: 'success', summary: 'Deleted' }); })
    });
  }
}
