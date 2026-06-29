export interface GalleryAlbum {
  id: number;
  title: string;
  description?: string;
  coverImageUrl?: string;
  category?: string;
  eventDate?: string;
  imageCount: number;
}

export interface GalleryImage {
  id: number;
  albumId: number;
  title: string;
  imageUrl: string;
  caption?: string;
  sortOrder: number;
}

export interface GalleryAlbumDetail {
  album: GalleryAlbum;
  images: GalleryImage[];
}

export interface AlbumForm {
  title: string;
  description: string;
  category: string;
  eventDate: string;
}

export interface ImageForm {
  title: string;
  imageUrl: string;
  caption: string;
  sortOrder: number;
}

export const defaultAlbumForm = (): AlbumForm => ({
  title: '',
  description: '',
  category: '',
  eventDate: '',
});

export const defaultImageForm = (): ImageForm => ({
  title: '',
  imageUrl: '',
  caption: '',
  sortOrder: 0,
});
