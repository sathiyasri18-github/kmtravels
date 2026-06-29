export interface NewsItem {
  id: number;
  title: string;
  slug: string;
  summary: string;
  content?: string;
  imageUrl?: string;
  category: number;
  isPublished: boolean;
  publishedAt?: string;
  author?: string;
  viewCount?: number;
}

export interface NewsForm {
  title: string;
  summary: string;
  content: string;
  imageUrl: string;
  category: number;
  isPublished: boolean;
  author: string;
}

export const defaultNewsForm = (): NewsForm => ({
  title: '',
  summary: '',
  content: '',
  imageUrl: '',
  category: 1,
  isPublished: false,
  author: '',
});
