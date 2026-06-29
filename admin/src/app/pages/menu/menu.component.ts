import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelectModule } from 'primeng/select';
import { CheckboxModule } from 'primeng/checkbox';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { MenuService } from '../../services/menu.service';
import { defaultMenuForm, MenuForm, MenuItem } from '../../models/menu.model';
import { SelectOption } from '../../models/common.model';

interface FlatMenuItem extends MenuItem {
  indent: string;
  parentTitle?: string;
}

@Component({
  selector: 'app-menu',
  standalone: true,
  providers: [ConfirmationService, MessageService],
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, DialogModule,
    InputTextModule, InputNumberModule, SelectModule, CheckboxModule,
    ConfirmDialogModule, ToastModule
  ],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.scss'
})
export class MenuComponent implements OnInit {
  menuTree: MenuItem[] = [];
  flatItems: FlatMenuItem[] = [];
  parentOptions: SelectOption<number | null>[] = [{ label: 'None (Root)', value: null }];
  dialogVisible = false;
  saving = false;
  editItem: MenuItem | null = null;
  form: MenuForm = defaultMenuForm();

  constructor(private service: MenuService, private confirm: ConfirmationService, private msg: MessageService) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.service.getAll().subscribe(tree => {
      this.menuTree = tree;
      this.flatItems = this.flatten(tree);
      this.parentOptions = [
        { label: 'None (Root)', value: null },
        ...this.flatItems.map(i => ({ label: i.title, value: i.id }))
      ];
    });
  }

  private flatten(items: MenuItem[], level = 0, parentTitle?: string): FlatMenuItem[] {
    const result: FlatMenuItem[] = [];
    for (const item of items) {
      result.push({ ...item, indent: '  '.repeat(level), parentTitle });
      if (item.children?.length) {
        result.push(...this.flatten(item.children, level + 1, item.title));
      }
    }
    return result;
  }

  openDialog(): void { this.editItem = null; this.form = defaultMenuForm(); this.dialogVisible = true; }

  edit(item: FlatMenuItem): void {
    this.editItem = item;
    this.form = {
      title: item.title, url: item.url || '', parentId: item.parentId ?? null,
      sortOrder: item.sortOrder, openInNewTab: item.openInNewTab,
    };
    this.dialogVisible = true;
  }

  save(): void {
    this.saving = true;
    const op = this.editItem ? this.service.update(this.editItem.id, this.form) : this.service.create(this.form);
    op.subscribe({
      next: () => { this.saving = false; this.dialogVisible = false; this.load(); this.msg.add({ severity: 'success', summary: 'Saved' }); },
      error: () => { this.saving = false; }
    });
  }

  confirmDelete(item: MenuItem): void {
    this.confirm.confirm({
      message: `Delete "${item.title}"?`, header: 'Confirm', icon: 'pi pi-exclamation-triangle',
      accept: () => this.service.delete(item.id).subscribe(() => { this.load(); this.msg.add({ severity: 'success', summary: 'Deleted' }); })
    });
  }
}
