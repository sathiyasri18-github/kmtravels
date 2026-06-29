import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { TagModule } from 'primeng/tag';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { EventService } from '../../services/event.service';
import { defaultEventForm, EventForm, EventItem } from '../../models/event.model';
import { toDateTimeLocal } from '../../models/common.model';
import { RichTextEditorComponent } from '../../shared/rich-text-editor/rich-text-editor.component';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';

@Component({
  selector: 'app-events',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, CheckboxModule, TagModule, ConfirmDialogModule,
    ToastModule, RichTextEditorComponent, FileUploadComponent
  ],
  templateUrl: './events.component.html',
  styleUrl: './events.component.scss'
})
export class EventsComponent implements OnInit {
  events: EventItem[] = [];
  dialogVisible = false;
  saving = false;
  editItem: EventItem | null = null;
  form: EventForm = defaultEventForm();

  constructor(private service: EventService, private confirm: ConfirmationService, private msg: MessageService) {}

  ngOnInit(): void { this.load(); }

  load(): void { this.service.getAll().subscribe(e => this.events = e); }

  openDialog(): void { this.editItem = null; this.form = defaultEventForm(); this.dialogVisible = true; }

  edit(item: EventItem): void {
    this.editItem = item;
    this.form = {
      title: item.title, description: item.description || '', location: item.location || '',
      startDate: toDateTimeLocal(item.startDate), endDate: toDateTimeLocal(item.endDate),
      imageUrl: item.imageUrl || '', isPublished: item.isPublished,
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

  confirmDelete(item: EventItem): void {
    this.confirm.confirm({
      message: `Delete "${item.title}"?`, header: 'Confirm', icon: 'pi pi-exclamation-triangle',
      accept: () => this.service.delete(item.id).subscribe(() => { this.load(); this.msg.add({ severity: 'success', summary: 'Deleted' }); })
    });
  }
}
