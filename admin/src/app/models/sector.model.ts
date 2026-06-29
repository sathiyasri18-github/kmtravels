export interface SectorImage {
  id?: number;
  sectorId?: number;
  title: string;
  imageUrl: string;
  caption?: string;
  sortOrder: number;
}

export interface SectorItem {
  id: number;
  title: string;
  subtitle?: string;
  slug: string;
  content?: string;
  coverImageUrl?: string;
  metaTitle?: string;
  metaDescription?: string;
  metaKeywords?: string;
  sortOrder: number;
  isPublished: boolean;
  isFeatured: boolean;
  imageCount: number;
  images?: SectorImage[];
}

export interface SectorForm {
  title: string;
  subtitle: string;
  content: string;
  coverImageUrl: string;
  metaTitle: string;
  metaDescription: string;
  metaKeywords: string;
  slug: string;
  sortOrder: number;
  isPublished: boolean;
  isFeatured: boolean;
  images: SectorImage[];
}

export function defaultSectorForm(): SectorForm {
  return {
    title: '',
    subtitle: '',
    content: '',
    coverImageUrl: '',
    metaTitle: '',
    metaDescription: '',
    metaKeywords: '',
    slug: '',
    sortOrder: 0,
    isPublished: true,
    isFeatured: false,
    images: []
  };
}

export function defaultSectorImage(sortOrder: number): SectorImage {
  return { title: '', imageUrl: '', caption: '', sortOrder };
}
