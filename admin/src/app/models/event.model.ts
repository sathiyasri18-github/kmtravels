export interface EventItem {
  id: number;
  title: string;
  description?: string;
  location?: string;
  startDate: string;
  endDate?: string;
  imageUrl?: string;
  isPublished: boolean;
}

export interface EventForm {
  title: string;
  description: string;
  location: string;
  startDate: string;
  endDate: string;
  imageUrl: string;
  isPublished: boolean;
}

export const defaultEventForm = (): EventForm => ({
  title: '',
  description: '',
  location: '',
  startDate: '',
  endDate: '',
  imageUrl: '',
  isPublished: true,
});
