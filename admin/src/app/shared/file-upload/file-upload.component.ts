import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { ApiService } from '../../core/api.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-file-upload',
  standalone: true,
  providers: [MessageService],
  imports: [FormsModule, ButtonModule, InputTextModule, ToastModule],
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss'
})
export class FileUploadComponent {
  @Input() folder = 'images';
  @Input() url = '';
  @Input() label = 'File URL';
  @Input() accept = 'image/*';
  @Output() urlChange = new EventEmitter<string>();

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  uploading = false;

  constructor(private api: ApiService, private msg: MessageService) {}

  triggerFilePicker(): void {
    this.fileInput.nativeElement.click();
  }

  get previewUrl(): string {
    if (!this.url) return '';
    if (this.url.startsWith('http://') || this.url.startsWith('https://')) return this.url;
    const origin = environment.apiOrigin ?? environment.apiUrl.replace(/\/api\/?$/, '');
    return `${origin}${this.url.startsWith('/') ? '' : '/'}${this.url}`;
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    this.uploading = true;
    this.api.upload(this.folder, file).subscribe({
      next: path => {
        this.url = path;
        this.urlChange.emit(path);
        this.uploading = false;
        input.value = '';
        this.msg.add({ severity: 'success', summary: 'Uploaded', detail: 'File uploaded successfully' });
      },
      error: err => {
        this.uploading = false;
        input.value = '';
        const detail = err?.error?.message ?? 'Upload failed. Check you are logged in and the API is running.';
        this.msg.add({ severity: 'error', summary: 'Upload failed', detail });
      }
    });
  }

  onUrlInput(value: string): void {
    this.url = value;
    this.urlChange.emit(value);
  }
}
