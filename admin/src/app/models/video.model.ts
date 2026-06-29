export interface VideoItem {
  id: number;
  title: string;
  description?: string;
  source: number;
  videoUrl?: string;
  youTubeId?: string;
  thumbnailUrl?: string;
  category?: string;
  isPublished: boolean;
}

export interface VideoForm {
  title: string;
  description: string;
  source: number;
  videoUrl: string;
  youTubeId: string;
  thumbnailUrl: string;
  category: string;
  isPublished: boolean;
}

export const defaultVideoForm = (): VideoForm => ({
  title: '',
  description: '',
  source: 2,
  videoUrl: '',
  youTubeId: '',
  thumbnailUrl: '',
  category: '',
  isPublished: true,
});
