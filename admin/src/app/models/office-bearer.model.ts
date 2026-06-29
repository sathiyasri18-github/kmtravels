export interface OfficeBearer {
  id: number;
  name: string;
  role: number;
  designation?: string;
  district?: string;
  phone?: string;
  email?: string;
  photoUrl?: string;
  sortOrder: number;
  bio?: string;
}

export interface OfficeBearerForm {
  name: string;
  role: number;
  designation: string;
  district: string;
  phone: string;
  email: string;
  photoUrl: string;
  sortOrder: number;
  bio: string;
}

export const defaultOfficeBearerForm = (): OfficeBearerForm => ({
  name: '',
  role: 1,
  designation: '',
  district: '',
  phone: '',
  email: '',
  photoUrl: '',
  sortOrder: 0,
  bio: '',
});
