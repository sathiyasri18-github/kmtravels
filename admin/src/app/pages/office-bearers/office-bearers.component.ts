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
import { OfficeBearerService } from '../../services/office-bearer.service';
import { defaultOfficeBearerForm, OfficeBearer, OfficeBearerForm } from '../../models/office-bearer.model';
import { OfficeBearerRoles, labelFor } from '../../models/enums.model';
import { RichTextEditorComponent } from '../../shared/rich-text-editor/rich-text-editor.component';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';

@Component({
  selector: 'app-office-bearers',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, InputNumberModule, SelectModule, TagModule,
    ConfirmDialogModule, ToastModule, RichTextEditorComponent, FileUploadComponent
  ],
  templateUrl: './office-bearers.component.html',
  styleUrl: './office-bearers.component.scss'
})
export class OfficeBearersComponent implements OnInit {
  bearers: OfficeBearer[] = [];
  dialogVisible = false;
  saving = false;
  editItem: OfficeBearer | null = null;
  form: OfficeBearerForm = defaultOfficeBearerForm();
  roleOptions = OfficeBearerRoles;

  constructor(private service: OfficeBearerService, private confirm: ConfirmationService, private msg: MessageService) {}

  ngOnInit(): void { this.load(); }

  getRoleLabel(v: number): string { return labelFor(OfficeBearerRoles, v); }

  load(): void { this.service.getAll().subscribe(b => this.bearers = b); }

  openDialog(): void { this.editItem = null; this.form = defaultOfficeBearerForm(); this.dialogVisible = true; }

  edit(item: OfficeBearer): void {
    this.editItem = item;
    this.form = {
      name: item.name, role: item.role, designation: item.designation || '',
      district: item.district || '', phone: item.phone || '', email: item.email || '',
      photoUrl: item.photoUrl || '', sortOrder: item.sortOrder, bio: item.bio || '',
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

  confirmDelete(item: OfficeBearer): void {
    this.confirm.confirm({
      message: `Delete "${item.name}"?`, header: 'Confirm', icon: 'pi pi-exclamation-triangle',
      accept: () => this.service.delete(item.id).subscribe(() => { this.load(); this.msg.add({ severity: 'success', summary: 'Deleted' }); })
    });
  }
}
