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
import { VideoService } from '../../services/video.service';
import { defaultVideoForm, VideoForm, VideoItem } from '../../models/video.model';
import { VideoSources, labelFor } from '../../models/enums.model';
import { RichTextEditorComponent } from '../../shared/rich-text-editor/rich-text-editor.component';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';

@Component({
  selector: 'app-videos',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, SelectModule, CheckboxModule, TagModule,
    ConfirmDialogModule, ToastModule, RichTextEditorComponent, FileUploadComponent
  ],
  templateUrl: './videos.component.html',
  styleUrl: './videos.component.scss'
})
export class VideosComponent implements OnInit {
  videos: VideoItem[] = [];
  dialogVisible = false;
  saving = false;
  editItem: VideoItem | null = null;
  form: VideoForm = defaultVideoForm();
  sourceOptions = VideoSources;

  constructor(private videoService: VideoService, private confirm: ConfirmationService, private msg: MessageService) {}

  ngOnInit(): void { this.load(); }

  getSourceLabel(v: number): string { return labelFor(VideoSources, v); }

  load(): void { this.videoService.getAll().subscribe(v => this.videos = v); }

  openDialog(): void { this.editItem = null; this.form = defaultVideoForm(); this.dialogVisible = true; }

  edit(item: VideoItem): void {
    this.editItem = item;
    this.form = {
      title: item.title,
      description: item.description || '',
      source: item.source,
      videoUrl: item.videoUrl || '',
      youTubeId: item.youTubeId || '',
      thumbnailUrl: item.thumbnailUrl || '',
      category: item.category || '',
      isPublished: item.isPublished,
    };
    this.dialogVisible = true;
  }

  save(): void {
    this.saving = true;
    const op = this.editItem ? this.videoService.update(this.editItem.id, this.form) : this.videoService.create(this.form);
    op.subscribe({
      next: () => { this.saving = false; this.dialogVisible = false; this.load(); this.msg.add({ severity: 'success', summary: 'Saved' }); },
      error: () => { this.saving = false; }
    });
  }

  confirmDelete(item: VideoItem): void {
    this.confirm.confirm({
      message: `Delete "${item.title}"?`, header: 'Confirm', icon: 'pi pi-exclamation-triangle',
      accept: () => this.videoService.delete(item.id).subscribe(() => { this.load(); this.msg.add({ severity: 'success', summary: 'Deleted' }); })
    });
  }
}
