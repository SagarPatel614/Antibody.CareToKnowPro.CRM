import { Injectable } from "@angular/core";
import { ClipboardService } from 'ngx-clipboard'
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';


@Injectable()
export class ClipboardServiceModule {

  constructor(private readonly clipboardService: ClipboardService,
    public dialog: MatDialog) {
  }

  public copy(text: string): boolean {
    return this.clipboardService.copyFromContent(text);
  }
}
