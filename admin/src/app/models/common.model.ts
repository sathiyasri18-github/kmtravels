export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface SelectOption<T = number | string> {
  label: string;
  value: T;
}

export function emptyForm<T extends object>(defaults: T): T {
  return { ...defaults };
}

export function toDateInput(value?: string | null): string {
  if (!value) return '';
  return value.substring(0, 10);
}

export function toDateTimeLocal(value?: string | null): string {
  if (!value) return '';
  const d = new Date(value);
  if (isNaN(d.getTime())) return '';
  return d.toISOString().slice(0, 16);
}
