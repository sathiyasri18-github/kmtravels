export interface ContactInquiry {
  id: number;
  name: string;
  email: string;
  phone?: string;
  subject: string;
  message: string;
  isRead: boolean;
  createdAt: string;
}

export interface PagedInquiries {
  items: ContactInquiry[];
  totalCount: number;
}
