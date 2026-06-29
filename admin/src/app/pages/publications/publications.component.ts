import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { CheckboxModule } from 'primeng/checkbox';
import { TagModule } from 'primeng/tag';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { PublicationService } from '../../services/publication.service';
import { defaultPublicationForm, Publication, PublicationForm } from '../../models/publication.model';
import { PublicationTypes, labelFor } from '../../models/enums.model';
import { toDateInput } from '../../models/common.model';
import { RichTextEditorComponent } from '../../shared/rich-text-editor/rich-text-editor.component';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';

@Component({
  selector: 'app-publications',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, SelectModule, CheckboxModule, TagModule,
    ConfirmDialogModule, ToastModule, RichTextEditorComponent, FileUploadComponent
  ],
  templateUrl: './publications.component.html',
  styleUrl: './publications.component.scss'
})
export class PublicationsComponent implements OnInit {
  publications: Publication[] = [];
  dialogVisible = false;
  saving = false;
  editItem: Publication | null = null;
  form: PublicationForm = defaultPublicationForm();
  typeOptions = PublicationTypes;

  constructor(private pubService: PublicationService, private confirm: ConfirmationService, private msg: MessageService) {}

  ngOnInit(): void { this.load(); }

  getTypeLabel(v: number): string { return labelFor(PublicationTypes, v); }

  load(): void { this.pubService.getAll().subscribe(p => this.publications = p); }

  openDialog(): void { this.editItem = null; this.form = defaultPublicationForm(); this.dialogVisible = true; }

  edit(item: Publication): void {
    this.editItem = item;
    this.form = {
      title: item.title, type: item.type, edition: item.edition || '',
      description: item.description || '', pdfUrl: item.pdfUrl,
      coverImageUrl: item.coverImageUrl || '', publishedDate: toDateInput(item.publishedDate),
      isPublished: item.isPublished,
    };
    this.dialogVisible = true;
  }

  save(): void {
    this.saving = true;
    const op = this.editItem ? this.pubService.update(this.editItem.id, this.form) : this.pubService.create(this.form);
    op.subscribe({
      next: () => { this.saving = false; this.dialogVisible = false; this.load(); this.msg.add({ severity: 'success', summary: 'Saved' }); },
      error: () => { this.saving = false; }
    });
  }

  confirmDelete(item: Publication): void {
    this.confirm.confirm({
      message: `Delete "${item.title}"?`, header: 'Confirm', icon: 'pi pi-exclamation-triangle',
      accept: () => this.pubService.delete(item.id).subscribe(() => { this.load(); this.msg.add({ severity: 'success', summary: 'Deleted' }); })
    });
  }
}
