export interface SiteSettings {
  associationName: string;
  tagline: string;
  address: string;
  phone: string;
  email: string;
  mapEmbedUrl: string;
  aboutContent: string;
  vision: string;
  mission: string;
  objectives: string;
}

export const defaultSiteSettings = (): SiteSettings => ({
  associationName: '',
  tagline: '',
  address: '',
  phone: '',
  email: '',
  mapEmbedUrl: '',
  aboutContent: '',
  vision: '',
  mission: '',
  objectives: '',
});
