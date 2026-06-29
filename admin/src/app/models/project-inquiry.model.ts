export interface ProjectInquiry {
  id: number;
  fullName: string;
  email: string;
  phone: string;
  companyName?: string;
  city?: string;
  state?: string;
  projectType?: string;
  projectDetails?: string;
  status: number;
  notes?: string;
  createdAt: string;
}

export interface ProjectInquiryForm {
  fullName: string;
  email: string;
  phone: string;
  companyName: string;
  city: string;
  state: string;
  projectType: string;
  projectDetails: string;
  status: number;
  notes: string;
}

export interface PagedProjectInquiries {
  items: ProjectInquiry[];
  totalCount: number;
}

export const PROJECT_INQUIRY_STATUS = [
  { label: 'New', value: 1 },
  { label: 'Reviewed', value: 2 },
  { label: 'Closed', value: 3 },
];

export function defaultProjectInquiryForm(): ProjectInquiryForm {
  return {
    fullName: '',
    email: '',
    phone: '',
    companyName: '',
    city: '',
    state: '',
    projectType: '',
    projectDetails: '',
    status: 1,
    notes: '',
  };
}
