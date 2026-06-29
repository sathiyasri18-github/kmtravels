export interface Advertisement {
  id: number;
  title: string;
  imageUrl: string;
  linkUrl?: string;
  position: number;
  startDate: string;
  endDate: string;
  sortOrder: number;
}

export interface AdvertisementForm {
  title: string;
  imageUrl: string;
  linkUrl: string;
  position: number;
  startDate: string;
  endDate: string;
  sortOrder: number;
}

export const defaultAdvertisementForm = (): AdvertisementForm => ({
  title: '',
  imageUrl: '',
  linkUrl: '',
  position: 1,
  startDate: new Date().toISOString().slice(0, 10),
  endDate: new Date(Date.now() + 30 * 86400000).toISOString().slice(0, 10),
  sortOrder: 0,
});
