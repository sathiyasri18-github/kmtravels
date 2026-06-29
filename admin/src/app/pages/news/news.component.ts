import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { CheckboxModule } from 'primeng/checkbox';
import { TagModule } from 'primeng/tag';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { NewsService } from '../../services/news.service';
import { defaultNewsForm, NewsForm, NewsItem } from '../../models/news.model';
import { NewsCategories, labelFor } from '../../models/enums.model';
import { RichTextEditorComponent } from '../../shared/rich-text-editor/rich-text-editor.component';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';

@Component({
  selector: 'app-news',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, TextareaModule, SelectModule, CheckboxModule, TagModule,
    ConfirmDialogModule, ToastModule, RichTextEditorComponent, FileUploadComponent
  ],
  templateUrl: './news.component.html',
  styleUrl: './news.component.scss'
})
export class NewsComponent implements OnInit {
  news: NewsItem[] = [];
  dialogVisible = false;
  saving = false;
  editItem: NewsItem | null = null;
  form: NewsForm = defaultNewsForm();
  categoryOptions = NewsCategories;

  constructor(
    private newsService: NewsService,
    private confirm: ConfirmationService,
    private msg: MessageService
  ) {}

  ngOnInit(): void { this.load(); }

  getCategoryLabel(value: number): string {
    return labelFor(NewsCategories, value);
  }

  load(): void {
    this.newsService.getAll().subscribe(r => this.news = r.items);
  }

  openDialog(): void {
    this.editItem = null;
    this.form = defaultNewsForm();
    this.dialogVisible = true;
  }

  edit(item: NewsItem): void {
    this.editItem = item;
    this.newsService.getById(item.id).subscribe(detail => {
      this.form = {
        title: detail.title,
        summary: detail.summary,
        content: detail.content || '',
        imageUrl: detail.imageUrl || '',
        category: detail.category,
        isPublished: detail.isPublished,
        author: detail.author || '',
      };
      this.dialogVisible = true;
    });
  }

  save(): void {
    this.saving = true;
    const op = this.editItem
      ? this.newsService.update(this.editItem.id, this.form)
      : this.newsService.create(this.form);

    op.subscribe({
      next: () => {
        this.saving = false;
        this.dialogVisible = false;
        this.load();
        this.msg.add({ severity: 'success', summary: 'Saved', detail: 'News article saved successfully' });
      },
      error: () => { this.saving = false; this.msg.add({ severity: 'error', summary: 'Error', detail: 'Failed to save' }); }
    });
  }

  togglePublish(item: NewsItem): void {
    this.newsService.togglePublish(item.id).subscribe(() => this.load());
  }

  confirmDelete(item: NewsItem): void {
    this.confirm.confirm({
      message: `Delete "${item.title}"?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.newsService.delete(item.id).subscribe(() => {
        this.load();
        this.msg.add({ severity: 'success', summary: 'Deleted' });
      })
    });
  }
}
