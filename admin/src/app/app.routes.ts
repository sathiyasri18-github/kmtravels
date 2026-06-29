import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';
import { LoginComponent } from './pages/login/login.component';
import { AdminLayoutComponent } from './layout/admin-layout.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { NewsComponent } from './pages/news/news.component';
import { ProjectInquiryComponent } from './pages/project-inquiry/project-inquiry.component';
import { GalleryComponent } from './pages/gallery/gallery.component';
import { VideosComponent } from './pages/videos/videos.component';
import { SectorsComponent } from './pages/sectors/sectors.component';
import { OfficeBearersComponent } from './pages/office-bearers/office-bearers.component';
import { AdvertisementsComponent } from './pages/advertisements/advertisements.component';
import { BannersComponent } from './pages/banners/banners.component';
import { EventsComponent } from './pages/events/events.component';
import { MenuComponent } from './pages/menu/menu.component';
import { PagesComponent } from './pages/pages/pages.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { ContactComponent } from './pages/contact/contact.component';
import { ServicesComponent } from './pages/services/services.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'news', component: NewsComponent },
      { path: 'services', component: ServicesComponent },
      { path: 'project-inquiry', component: ProjectInquiryComponent },
      { path: 'gallery', component: GalleryComponent },
      { path: 'videos', component: VideosComponent },
      { path: 'sectors', component: SectorsComponent },
      { path: 'office-bearers', component: OfficeBearersComponent },
      { path: 'advertisements', component: AdvertisementsComponent },
      { path: 'banners', component: BannersComponent },
      { path: 'events', component: EventsComponent },
      { path: 'contact', component: ContactComponent },
      { path: 'menu', component: MenuComponent },
      { path: 'pages', component: PagesComponent },
      { path: 'settings', component: SettingsComponent },
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];
