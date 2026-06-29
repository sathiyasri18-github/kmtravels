import { SelectOption } from './common.model';

export const NewsCategories: SelectOption[] = [
  { label: 'Latest News', value: 1 },
  { label: 'Press Release', value: 2 },
  { label: 'Industry Update', value: 3 },
  { label: 'Government Announcement', value: 4 },
];

export const VideoSources: SelectOption[] = [
  { label: 'Upload', value: 1 },
  { label: 'YouTube', value: 2 },
];

export const PublicationTypes: SelectOption[] = [
  { label: 'Magazine', value: 1 },
  { label: 'Circular', value: 2 },
  { label: 'Newsletter', value: 3 },
];

export const OfficeBearerRoles: SelectOption[] = [
  { label: 'President', value: 1 },
  { label: 'Secretary', value: 2 },
  { label: 'Treasurer', value: 3 },
  { label: 'District Coordinator', value: 4 },
  { label: 'Committee Member', value: 5 },
];

export const AdvertisementPositions: SelectOption[] = [
  { label: 'Home Banner', value: 1 },
  { label: 'Sidebar', value: 2 },
  { label: 'Footer', value: 3 },
  { label: 'Popup', value: 4 },
];

export function labelFor(options: SelectOption[], value: number): string {
  return options.find(o => o.value === value)?.label ?? String(value);
}
