export interface ServiceImage {
  id?: number;
  serviceId?: number;
  title: string;
  imageUrl: string;
  caption?: string;
  sortOrder: number;
}

export interface ServiceItem {
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
  demandRating: number;
  isPublished: boolean;
  isFeatured: boolean;
  imageCount: number;
  images?: ServiceImage[];
}

export interface ServiceForm {
  title: string;
  subtitle: string;
  content: string;
  coverImageUrl: string;
  metaTitle: string;
  metaDescription: string;
  metaKeywords: string;
  slug: string;
  sortOrder: number;
  demandRating: number;
  isPublished: boolean;
  isFeatured: boolean;
  images: ServiceImage[];
}

export function defaultServiceForm(): ServiceForm {
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
    demandRating: 5,
    isPublished: true,
    isFeatured: false,
    images: []
  };
}

export function defaultServiceImage(sortOrder: number): ServiceImage {
  return { title: '', imageUrl: '', caption: '', sortOrder };
}
