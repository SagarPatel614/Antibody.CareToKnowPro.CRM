import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { ILoginProfile as LoginProfile } from '../model/models';
import { NotificationService } from '../service/notifications';
import { LoginProfilesService, UsersService } from '../api/api';
import { Subscription, BehaviorSubject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ExportToCsv } from 'export-to-csv';
import * as moment from 'moment';

@Component({
  selector: 'app-export-dialog',
  templateUrl: './exportDialog.component.html',
  styleUrls: ['./exportDialog.component.css']
})
export class ExportdDialogComponent implements OnInit {

  exportForm: FormGroup;
  selections: any;
  loading = false;

  constructor(private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ExportdDialogComponent>,
    private toasterService: NotificationService,
    private loginProfileService: LoginProfilesService,
    private userApi: UsersService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.selections = this.getSelection();

    this.exportForm = this.formBuilder.group({
      currentPageOnly: [data.currentPageOnly, Validators.required]
    });

  }

  get f() { return this.exportForm.controls; }

  ngOnInit() {

  }

  getSelection() {
    return [
      { id: '', name: '' },
      { id: true, name: 'Current Page' },
      { id: false, name: 'All Available Pages' }
    ];
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {

    if (this.exportForm.invalid) {
      return;
    }

    this.loading = true;

    let fileName = "HCP_" + moment().local().format("LLL");
    const options = {
      fieldSeparator: ',',
      quoteStrings: '"',
      decimalSeparator: '.',
      showLabels: true,
      showTitle: true,
      title: 'HCP Record(s)',
      useTextFile: false,
      useBom: true,
      useKeysAsHeaders: false,
      filename: fileName,
      headers: ['FirstName', 'LastName', 'Email', 'SecondaryEmail(s)', 'Address', 'Phone', 'Fax', 'AdditionalInfo', 'Specialty', 'OtherSpecialty', 'Graduation Year', 'Email Status', 'Status', 'Language', 'CreatedBy', 'DateCreated', 'ModifiedBy', 'DateModified']
    };

    const csvExporter = new ExportToCsv(options);

    var data = {
      filter: this.data.filter,
      sort: this.data.sort,
      SortDirection: this.data.SortDirection,
      currentPageIndex: this.data.currentPageIndex,
      PageSize: this.data.PageSize,
      OnlyCurrentPage: this.f.currentPageOnly.value,
    }

    this.userApi.apiPagedUsersForExport(data).subscribe(data => {
      csvExporter.generateCsv(data);
      this.toasterService.success("Export process completed successfully");
      this.dialogRef.close();
      this.loading = false;
    },
    error => {
        this.dialogRef.close();
        this.loading = false;
      })
  }
}
