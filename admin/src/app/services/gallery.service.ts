import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { AlbumForm, GalleryAlbum, GalleryAlbumDetail, GalleryImage, ImageForm } from '../models/gallery.model';

@Injectable({ providedIn: 'root' })
export class GalleryService {
  constructor(private api: ApiService) {}

  getAlbums(): Observable<GalleryAlbum[]> {
    return this.api.get<GalleryAlbum[]>('/admin/gallery');
  }

  getAlbumDetail(id: number): Observable<GalleryAlbumDetail> {
    return this.api.get<GalleryAlbumDetail>(`/public/gallery/${id}`);
  }

  createAlbum(form: AlbumForm): Observable<unknown> {
    return this.api.post('/admin/gallery', {
      title: form.title,
      description: form.description || null,
      category: form.category || null,
      eventDate: form.eventDate || null,
    });
  }

  updateAlbum(id: number, form: AlbumForm): Observable<unknown> {
    return this.api.put(`/admin/gallery/${id}`, {
      title: form.title,
      description: form.description || null,
      category: form.category || null,
      eventDate: form.eventDate || null,
    });
  }

  deleteAlbum(id: number): Observable<boolean> {
    return this.api.delete(`/admin/gallery/${id}`);
  }

  addImage(albumId: number, form: ImageForm): Observable<GalleryImage> {
    return this.api.post<GalleryImage>(`/admin/gallery/${albumId}/images`, {
      id: 0,
      albumId,
      title: form.title,
      imageUrl: form.imageUrl,
      caption: form.caption || null,
      sortOrder: form.sortOrder,
    });
  }

  deleteImage(imageId: number): Observable<boolean> {
    return this.api.delete(`/admin/gallery/images/${imageId}`);
  }
}
