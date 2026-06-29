export interface Publication {
  id: number;
  title: string;
  type: number;
  edition?: string;
  description?: string;
  pdfUrl: string;
  coverImageUrl?: string;
  publishedDate?: string;
  isPublished: boolean;
}

export interface PublicationForm {
  title: string;
  type: number;
  edition: string;
  description: string;
  pdfUrl: string;
  coverImageUrl: string;
  publishedDate: string;
  isPublished: boolean;
}

export const defaultPublicationForm = (): PublicationForm => ({
  title: '',
  type: 1,
  edition: '',
  description: '',
  pdfUrl: '',
  coverImageUrl: '',
  publishedDate: '',
  isPublished: true,
});
