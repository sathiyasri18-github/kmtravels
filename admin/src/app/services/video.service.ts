import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../core/api.service';
import { VideoForm, VideoItem } from '../models/video.model';

@Injectable({ providedIn: 'root' })
export class VideoService {
  constructor(private api: ApiService) {}

  getAll(): Observable<VideoItem[]> {
    return this.api.get<VideoItem[]>('/admin/videos');
  }

  create(form: VideoForm): Observable<unknown> {
    return this.api.post('/admin/videos', this.toRequest(form));
  }

  update(id: number, form: VideoForm): Observable<unknown> {
    return this.api.put(`/admin/videos/${id}`, this.toRequest(form));
  }

  delete(id: number): Observable<boolean> {
    return this.api.delete(`/admin/videos/${id}`);
  }

  private toRequest(form: VideoForm) {
    return {
      title: form.title,
      description: form.description || null,
      source: form.source,
      videoUrl: form.videoUrl || null,
      youTubeId: form.youTubeId || null,
      thumbnailUrl: form.thumbnailUrl || null,
      category: form.category || null,
      isPublished: form.isPublished,
    };
  }
}
