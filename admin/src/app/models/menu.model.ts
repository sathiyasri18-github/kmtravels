export interface MenuItem {
  id: number;
  title: string;
  url?: string;
  parentId?: number | null;
  sortOrder: number;
  openInNewTab: boolean;
  children?: MenuItem[];
}

export interface MenuForm {
  title: string;
  url: string;
  parentId: number | null;
  sortOrder: number;
  openInNewTab: boolean;
}

export const defaultMenuForm = (): MenuForm => ({
  title: '',
  url: '',
  parentId: null,
  sortOrder: 0,
  openInNewTab: false,
});
