import { Component, AfterViewInit, OnInit, OnDestroy, Input, Output, EventEmitter, Optional, Inject, ViewChild } from "@angular/core";
import { Subscription, of } from "rxjs";
import { AuthService } from "../service/services";
import { UsersService } from "../api/api";
import { Router, ActivatedRoute } from "@angular/router";
import { NotificationService } from "../service/notifications";
import { MatDialog } from "@angular/material/dialog";
import { SharedService } from "../service/shared.service";
import { ExportToCsv } from "export-to-csv";
import { HttpRequest, HttpEventType, HttpErrorResponse, HttpClient } from "@angular/common/http";
import { tap, last, catchError, map } from "rxjs/operators";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { trigger, state, transition, animate, style } from "@angular/animations";
import { IDuplicateCheckResponse as DuplicateCheckResponse, FileUploadModel, IDuplicateCheckRecord as DuplicateCheckRecord } from "../model/duplicateCheckResponse";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";


@Component({
  selector: 'app-recordcheck',
  templateUrl: './recordcheck.component.html',
  styleUrls: ['./recordcheck.component.css'],
  animations: [
    trigger('fadeInOut', [
          state('in', style({ opacity: 100 })),
          transition('* => void', [
                animate(300, style({ opacity: 0 }))
          ])
    ])
]
})
export class RecordCheckComponent implements AfterViewInit, OnInit, OnDestroy {

  @Input() text = 'Upload';
  /** Name used in form which will be sent in HTTP request. */
  @Input() param = 'file';
  /** Target URL for file uploading. */
  @Input() target = 'api/DuplicateUserCheck';
  /** File extension that accepted, same as 'accept' of <input type="file" />.
      By the default, it's set to '.csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel'. */
  @Input() accept = '.csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel';
  /** Allow you to add handler after its completion. Bubble up response text from remote. */
  @Output() complete = new EventEmitter<DuplicateCheckResponse>();

  files: Array<FileUploadModel> = [];

  matched: Array<DuplicateCheckRecord> = [];
  notmatched: Array<DuplicateCheckRecord> = [];

  subscriptions: Subscription[] = [];

displayedColumns: string[] = ['firstName', 'lastName', 'email', 'provinceCode', 'specialties', 'graduationYear', 'language'];
matchedDataSource: MatTableDataSource<DuplicateCheckRecord>;

@ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
@ViewChild(MatSort, {static: true}) sort: MatSort;
filled: boolean = false;
exportLoader = false;
exportLoader1 = false;
dataSource: MatTableDataSource<DuplicateCheckRecord>;

@ViewChild(MatPaginator, {static: true}) paginator1: MatPaginator;
@ViewChild(MatSort, {static: true}) sort1: MatSort;


  protected basePath = '/';

  constructor(
    private authenticationService: AuthService,
    private userApi: UsersService,
    private router: Router,
    private route: ActivatedRoute,
    private toasterService: NotificationService,
    public dialog: MatDialog,
    private sharedService: SharedService,
    private _http: HttpClient,
    @Optional() @Inject('BASE_URL') basePath: string
  ) {

    if (basePath) {
      this.basePath = basePath.charAt(basePath.length - 1) === "/" ? basePath.substring(0, basePath.length - 1) : basePath;
    }
    this.target = this.basePath + "/api/Users/DuplicateUserCheck";

    this.dataSource = new MatTableDataSource(this.matched);
    this.matchedDataSource = new MatTableDataSource(this.notmatched);
  }
  ngOnDestroy(): void {
  }

  DownloadTemplate() {

    const options = {
      fieldSeparator: ',',
      quoteStrings: '"',
      decimalSeparator: '.',
      showLabels: true,
     // showTitle: true,
     // title: 'HCP Record(s)',
      useTextFile: false,
      useBom: true,
      useKeysAsHeaders: false,
      filename: "HCPRecordCheck",
      headers: ['First Name', 'Last Name', 'Email', 'Province Code', "Specialties", 'Graduation Year', 'Language']
    };

    let data = [{
      firstName: " ",
      lastName: " ",
      email : " ",
      provinceCode: " ",
      specialty: " ",
      graduationYear: " ",
      language: "EN | FR"
    }]

    const csvExporter = new ExportToCsv(options);
    csvExporter.generateCsv(data);
    this.toasterService.success("Template downloaded successfully");
  }

  ngAfterViewInit() {
  }

  ngOnInit() {
    this.matchedDataSource.paginator = this.paginator;
    this.matchedDataSource.sort = this.sort;

    this.dataSource.paginator = this.paginator1;
    this.dataSource.sort = this.sort1;
  }

  onClick() {
    const fileUpload = document.getElementById('fileUpload') as HTMLInputElement;
    fileUpload.onchange = () => {
          for (let index = 0; index < fileUpload.files.length; index++) {
                const file = fileUpload.files[index];
                this.files.push({ data: file, state: 'in',
                  inProgress: false, progress: 0, canRetry: false, canCancel: true });
          }
          this.uploadFiles();
    };
    fileUpload.click();
}

cancelFile(file: FileUploadModel) {
    file.sub.unsubscribe();
    this.removeFileFromArray(file);
}

retryFile(file: FileUploadModel) {
    this.uploadFile(file);
    file.canRetry = false;
}

private uploadFiles() {
  const fileUpload = document.getElementById('fileUpload') as HTMLInputElement;
  fileUpload.value = '';

  this.files.forEach(file => {
        this.uploadFile(file);
  });
}

private removeFileFromArray(file: FileUploadModel) {
  const index = this.files.indexOf(file);
  if (index > -1) {
        this.files.splice(index, 1);
  }
}

private uploadFile(file: FileUploadModel) {

  this.filled = false;

  const fd = new FormData();
  fd.append(this.param, file.data);

  const req = new HttpRequest('POST', this.target, fd, {
        reportProgress: true
  });

  file.inProgress = true;
  file.sub = this._http.request(req).pipe(
        map(event => {
              switch (event.type) {
                    case HttpEventType.UploadProgress:
                          file.progress = Math.round(event.loaded * 100 / event.total);
                          break;
                    case HttpEventType.Response:
                          return event;
              }
        }),
        tap(message => { }),
        last(),
        catchError((error: HttpErrorResponse) => {
              file.inProgress = false;
              file.canRetry = true;
              this.toasterService.error(`${file.data.name} upload failed.`);
              return of(`${file.data.name} upload failed.`);
        })
  ).subscribe(
        (event: any) => {
              if (typeof (event) === 'object') {
                 //   this.removeFileFromArray(file);
                   // this.complete.emit(event.body);

                    this.matched = event.body.existingRecords;
                    this.notmatched = event.body.notExistingRecords;
                    this.matchedDataSource = new MatTableDataSource(this.matched);
                    this.dataSource = new MatTableDataSource(this.notmatched);
                    this.filled = true;
                    this.toasterService.success(`${file.data.name} reviewd successfully`);
              }
        }
  );
}

applyFilter(event: Event) {
  const filterValue = (event.target as HTMLInputElement).value;
  this.matchedDataSource.filter = filterValue.trim().toLowerCase();

  if (this.matchedDataSource.paginator) {
    this.matchedDataSource.paginator.firstPage();
  }
}

applyFilter1(event: Event) {
  const filterValue = (event.target as HTMLInputElement).value;
  this.dataSource.filter = filterValue.trim().toLowerCase();

  if (this.dataSource.paginator) {
    this.dataSource.paginator.firstPage();
  }
}

MatchedExportToCSV() {

  this.exportLoader = true;

  const options = {
    fieldSeparator: ',',
    quoteStrings: '"',
    decimalSeparator: '.',
    showLabels: true,
   // showTitle: true,
   // title: 'HCP Record(s)',
    useTextFile: false,
    useBom: true,
    useKeysAsHeaders: false,
    filename: "HCPRecordCheck",
    headers: ['First Name', 'Last Name', 'Email', 'Province Code', "Specialties", 'Graduation Year', 'Language']
  };

  let data = [];

  this.matched.forEach(element => {
    data.push({
      firstName: element.firstName,
      lastName: element.lastName,
      email : element.email,
      provinceCode: element.provinceCode,
      specialty: element.specialties,
      graduationYear: element.graduationYear,
      language: element.lastName
    })
  });

  const csvExporter = new ExportToCsv(options);
  csvExporter.generateCsv(data);
  this.toasterService.success("File downloaded successfully");
  this.exportLoader = false;
}

NotMatchedExportToCSV() {

  this.exportLoader1 = true;

  const options = {
    fieldSeparator: ',',
    quoteStrings: '"',
    decimalSeparator: '.',
    showLabels: true,
   // showTitle: true,
   // title: 'HCP Record(s)',
    useTextFile: false,
    useBom: true,
    useKeysAsHeaders: false,
    filename: "HCPRecordCheck",
    headers: ['First Name', 'Last Name', 'Email', 'Province Code', "Specialties", 'Graduation Year', 'Language']
  };

  let data = [];

  this.notmatched.forEach(element => {
    data.push({
      firstName: element.firstName,
      lastName: element.lastName,
      email : element.email,
      provinceCode: element.provinceCode,
      specialty: element.specialties,
      graduationYear: element.graduationYear,
      language: element.lastName
    })
  });

  const csvExporter = new ExportToCsv(options);
  csvExporter.generateCsv(data);
  this.toasterService.success("File downloaded successfully");
  this.exportLoader1 = false;
}

}
