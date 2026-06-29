import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { SettingsService } from '../../services/settings.service';
import { defaultSiteSettings, SiteSettings } from '../../models/settings.model';
import { RichTextEditorComponent } from '../../shared/rich-text-editor/rich-text-editor.component';

@Component({
  selector: 'app-settings',
  standalone: true,
  providers: [MessageService],
  imports: [
    CommonModule, FormsModule, CardModule, ButtonModule,
    InputTextModule, TextareaModule, ToastModule, RichTextEditorComponent
  ],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent implements OnInit {
  form: SiteSettings = defaultSiteSettings();
  loading = true;
  saving = false;

  constructor(private service: SettingsService, private msg: MessageService) {}

  ngOnInit(): void {
    this.service.get().subscribe({
      next: s => { this.form = { ...defaultSiteSettings(), ...s }; this.loading = false; },
      error: () => { this.loading = false; }
    });
  }

  save(): void {
    this.saving = true;
    this.service.update(this.form).subscribe({
      next: () => {
        this.saving = false;
        this.msg.add({ severity: 'success', summary: 'Settings saved', detail: 'Site settings updated successfully' });
      },
      error: () => { this.saving = false; this.msg.add({ severity: 'error', summary: 'Error', detail: 'Failed to save settings' }); }
    });
  }
}
