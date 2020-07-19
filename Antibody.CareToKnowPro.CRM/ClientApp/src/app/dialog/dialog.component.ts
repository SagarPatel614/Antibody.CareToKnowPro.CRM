import { Component, Inject } from '@angular/core';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent {

  message: string;
  isClipboard: boolean = false;
  isPasswordModule: boolean = false;
  displayedColumns = ['Id', 'Link', 'Copy'];

  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private toasterService: ToastrService) {
    this.message = data.message;
    this.isClipboard = data.isClipboard ? data.isClipboard : false;
    this.isPasswordModule = data.isPasswordModule ? data.isPasswordModule : false;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}

function copyTextToClipboard(text): Observable<boolean> {

  return new Observable(subscriber => {

    let newNavigator: any = window.navigator;
    if (!newNavigator.clipboard) {
      subscriber.next(fallbackCopyTextToClipboard(text));
      subscriber.complete();
    }

    newNavigator.clipboard.writeText(text).then(() => {
      console.log('Async: Copying to clipboard was successful!');
      subscriber.next(true);
      subscriber.complete();
    }, error => {
        //try again it could be due to the DOM is not ready yet
        //https://developers.google.com/web/updates/2018/03/clipboardapi
        setTimeout(async () => {
          newNavigator.clipboard.writeText(text).then(() => {
            subscriber.next(true);
            subscriber.complete();
          }, error => {
            console.error('Async: Could not copy text: ', error);
            subscriber.next(false);
            subscriber.complete();
          });
        }, 3000);
    });
  });
};

function fallbackCopyTextToClipboard(text): boolean {
  var textArea = document.createElement("textarea");
  textArea.value = text;
  textArea.style.position = "fixed";  //avoid scrolling to bottom
  document.body.appendChild(textArea);
  textArea.focus();
  textArea.select();

  var copySucceeded: boolean = false;
  try {
    copySucceeded = document.execCommand('copy');
    var msg = copySucceeded ? 'successful' : 'unsuccessful';
    console.log('Fallback: Copying text command was ' + msg);
  } catch (err) {
    console.error('Fallback: Oops, unable to copy', err);
  }

  document.body.removeChild(textArea);
  return copySucceeded;
}

export class DialogData {
  message: string;
  isClipboard: boolean;
  isPasswordModule: boolean;
}

