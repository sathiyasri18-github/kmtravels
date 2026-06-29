export interface DynamicPage {
  id: number;
  title: string;
  slug: string;
  content: string;
  metaDescription?: string;
  isPublished: boolean;
}

export interface PageForm {
  title: string;
  slug: string;
  content: string;
  metaDescription: string;
  isPublished: boolean;
}

export const defaultPageForm = (): PageForm => ({
  title: '',
  slug: '',
  content: '',
  metaDescription: '',
  isPublished: true,
});
