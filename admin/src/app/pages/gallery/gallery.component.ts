import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { GalleryService } from '../../services/gallery.service';
import {
  AlbumForm, defaultAlbumForm, defaultImageForm, GalleryAlbum,
  GalleryImage, ImageForm
} from '../../models/gallery.model';
import { toDateInput } from '../../models/common.model';
import { RichTextEditorComponent } from '../../shared/rich-text-editor/rich-text-editor.component';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';

@Component({
  selector: 'app-gallery',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, InputNumberModule, TooltipModule, ConfirmDialogModule,
    ToastModule, RichTextEditorComponent, FileUploadComponent
  ],
  templateUrl: './gallery.component.html',
  styleUrl: './gallery.component.scss'
})
export class GalleryComponent implements OnInit {
  albums: GalleryAlbum[] = [];
  images: GalleryImage[] = [];
  albumDialogVisible = false;
  imagesDialogVisible = false;
  saving = false;
  addingImage = false;
  editAlbumItem: GalleryAlbum | null = null;
  selectedAlbum: GalleryAlbum | null = null;
  albumForm: AlbumForm = defaultAlbumForm();
  imageForm: ImageForm = defaultImageForm();

  constructor(
    private galleryService: GalleryService,
    private confirm: ConfirmationService,
    private msg: MessageService
  ) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.galleryService.getAlbums().subscribe(a => this.albums = a);
  }

  openAlbumDialog(): void {
    this.editAlbumItem = null;
    this.albumForm = defaultAlbumForm();
    this.albumDialogVisible = true;
  }

  editAlbum(album: GalleryAlbum): void {
    this.editAlbumItem = album;
    this.albumForm = {
      title: album.title,
      description: album.description || '',
      category: album.category || '',
      eventDate: toDateInput(album.eventDate),
    };
    this.albumDialogVisible = true;
  }

  saveAlbum(): void {
    this.saving = true;
    const op = this.editAlbumItem
      ? this.galleryService.updateAlbum(this.editAlbumItem.id, this.albumForm)
      : this.galleryService.createAlbum(this.albumForm);

    op.subscribe({
      next: () => {
        this.saving = false;
        this.albumDialogVisible = false;
        this.load();
        this.msg.add({ severity: 'success', summary: 'Album saved' });
      },
      error: () => { this.saving = false; }
    });
  }

  confirmDeleteAlbum(album: GalleryAlbum): void {
    this.confirm.confirm({
      message: `Delete album "${album.title}"?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      accept: () => this.galleryService.deleteAlbum(album.id).subscribe(() => {
        this.load();
        this.msg.add({ severity: 'success', summary: 'Deleted' });
      })
    });
  }

  manageImages(album: GalleryAlbum): void {
    this.selectedAlbum = album;
    this.imageForm = defaultImageForm();
    this.loadImages(album.id);
    this.imagesDialogVisible = true;
  }

  loadImages(albumId: number): void {
    this.galleryService.getAlbumDetail(albumId).subscribe(d => this.images = d.images);
  }

  addImage(): void {
    if (!this.selectedAlbum || !this.imageForm.imageUrl) return;
    this.addingImage = true;
    this.galleryService.addImage(this.selectedAlbum.id, this.imageForm).subscribe({
      next: () => {
        this.addingImage = false;
        this.imageForm = defaultImageForm();
        this.loadImages(this.selectedAlbum!.id);
        this.load();
        this.msg.add({ severity: 'success', summary: 'Image added' });
      },
      error: () => { this.addingImage = false; }
    });
  }

  deleteImage(img: GalleryImage): void {
    this.galleryService.deleteImage(img.id).subscribe(() => {
      if (this.selectedAlbum) this.loadImages(this.selectedAlbum.id);
      this.load();
      this.msg.add({ severity: 'success', summary: 'Image deleted' });
    });
  }
}
