import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import * as Chartist from 'chartist';
import { AuthService } from '../service/services';
import { ILoginProfile as LoginProfile, IUser as User, IUserSpecialty as UserSpecialty, IUserUnsubscribe as UserUnsubscribe } from 'src/app/model/models';
import { Subscription, merge } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { UsersService } from '../api/api';
import { Router, ActivatedRoute } from '@angular/router';
import { DialogComponent } from '../dialog/dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { error, $ } from 'protractor';
import { NotificationService } from '../service/notifications';
import { FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { tap } from 'rxjs/operators';
import { MatSort } from '@angular/material/sort';
import { ExportToCsv } from 'export-to-csv';
import { formatDate } from '@angular/common';
import * as moment from 'moment';
import { ExportdDialogComponent } from '../dialog/exportDialog.component';
import { SharedService } from '../service/shared.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
 // styleUrls: ['./mat-table-responsive.directive.scss']
})
export class DashboardComponent implements AfterViewInit, OnInit, OnDestroy {

  currentUser: LoginProfile;
  currentUserSubscription: Subscription;
  UserData: any = [];
  userDataSource: MatTableDataSource<User>;

  displayedColumns: string[] = ['firstName', 'lastName', 'email', 'address', 'userSpecialty', 'graduationYear', 'emailstatus', 'status', 'preferredLanguage', 'actions'];
  isDatafilled = false;
  loading = false;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  //@ViewChild(MatSort, {static: true}) sort: MatSort;

  filter: any;

  provinces = [];
  languages: any;
  emailStatusList: any;
  statusList: any
  isFilterfilled = false;
  exportLoader = false;
  filterForm: FormGroup;

  pageIndex: number;
  pageSize: number;
  length: number;

  subscriptions: Subscription[] = [];

  constructor(
    private authenticationService: AuthService,
    private formBuilder: FormBuilder,
    private userApi: UsersService,
    private router: Router,
    private route: ActivatedRoute,
    private toasterService: NotificationService,
    public dialog: MatDialog,
    private sharedService: SharedService
  ) {


    this.languages = this.getLanguages();
    this.emailStatusList = this.getEmailStatusList();
    this.statusList = this.getStatusList();

    this.currentUserSubscription = this.authenticationService.currentUser.subscribe(user => {
      this.currentUser = user;
    });

    let data = {
      filter: this.sharedService.currentformValue.value,
      sort: '',
      SortDirection: '',
      currentPageIndex: 1,
      PageSize: 25
    }

    this.userApi.apiPagedUsers(data).subscribe(data => {
      this.isDatafilled = true;
      this.UserData = data.results;

      setTimeout(() => {
        this.length = data.totalCount;
        this.userDataSource = new MatTableDataSource<User>(this.UserData);
        this.userDataSource.sort = this.sort;
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

        if(this.paginator){
          this.paginator.length = data.totalCount;
          this.paginator.pageIndex = this.pageIndex;
        }

        merge(this.sort.sortChange, this.paginator.page)
          .pipe(
            tap(() => this.loadUserPage())
          )
          .subscribe();

      }, 0);
    })

    this.userApi.Getfilter().subscribe(filter => {
      this.isFilterfilled = true;
      this.filter = filter;
    })

  }
  ngOnDestroy(): void {
    this.sort?.sortChange?.unsubscribe();
    this.paginator.page.unsubscribe();
  }

  exportToCSV(onlyCurrentPage: boolean = false) {

    this.exportLoader = true;

    if(this.paginator){
      this.paginator.pageIndex = 0;
    }
    let data = {
      filter: this.filterForm.value,
      sort: this.sort?.active,
      SortDirection: this.sort?.direction,
      currentPageIndex: this.paginator?.pageIndex + 1,
      PageSize: this.paginator?.pageSize,
      OnlyCurrentPage: onlyCurrentPage
    }


    const dialogRef = this.dialog.open(ExportdDialogComponent, {
      width: '500px',
      data: data
    });
    dialogRef.afterClosed().subscribe(result => {
      this.exportLoader = false;
      if (result) {
        //this.toasterService.success("");
      }
    });
  }

  ngAfterViewInit() {
  }

  loadUserPage(filterFormSubmit: boolean = false) {

    if(filterFormSubmit && this.paginator){
      this.paginator.pageIndex = 0;
    }

    //set the form in shared service to preserve all the filter value
    this.sharedService.SetForm(this.filterForm);

    this.isDatafilled = false;
    let data = {
      filter: this.filterForm.value,
      sort: this.sort?.active,
      SortDirection: this.sort?.direction,
      currentPageIndex: this.paginator?.pageIndex + 1,
      PageSize: this.paginator?.pageSize
    }
    this.userApi.apiPagedUsers(data).subscribe(data => {
      this.isDatafilled = true;
      this.isDatafilled = true;
      this.UserData = data.results;
      setTimeout(() => {
        this.userDataSource = new MatTableDataSource<User>(this.UserData);
        if(this.paginator){
          this.paginator.length = data.totalCount;
          this.paginator.pageIndex = this.paginator.pageIndex;
        }
      }, 0);
    },
      error => {
        this.isDatafilled = true;
      })
  }

  ngOnInit() {
    let form = this.sharedService.currentformValue;
    this.filterForm = form;
    this.pageIndex = 0;
    this.pageSize = 25;
  }

  getLanguages() {
    return [
      { id: '', name: '' },
      { id: 'EN', name: 'English' },
      { id: 'FR', name: 'French' }
    ];
  }

  getEmailStatusList() {
    return [
      { id: "", name: '' },
      { id: "Subscribed", name: 'Subscribed' },
      { id: "Unsubscribed", name: 'Unsubscribed' }
      // { id: "Verified", name: 'Verified' }
    ];
  }

  getStatusList() {
    return [
      { id: "", name: '' },
      { id: "Practicing", name: 'Practicing' },
      { id: "Retired", name: 'Retired' },
      { id: "Moved", name: 'Moved' }
    ];
  }

  getAddress(user: User) {
    var street1 = user.street1 ? user.street1 + ', ' : "";
    var city = user.city ? user.city + ', ' : "";
    var province = user.province ? user.province.abbreviation + ', ' : "";
    var country = user.country ? user.country + ', ' : "";
    var postal = user.postal ? user.postal + ', ' : "";

    var address = street1 + city + province + country + postal
    address = address.trim().substring(0, address.length - 2);
    return address
  }

  getSpecialityString(specialties: Array<UserSpecialty>) {
    var output = specialties.map(x =>  `${x.speciality?.specialtyNameEn} ${x.specialtyOther? " (" + x.specialtyOther + ")" : ""}`).join(", ");
    return output;
  }

  getUnsubscribeString(specialties: Array<UserUnsubscribe>) {
    return specialties.map(x => x.reason?.reason).join(", ");
  }

  deleteUser(index: number, e) {
    this.loading = true;
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '500px',
      data: { message: "This will also delete this record and all their activities from Customer IO. Are you sure you want to delete?" }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const data = this.userDataSource.data;
        data.splice((this.paginator.pageIndex * this.paginator.pageSize) + index, 1);
        this.userDataSource.data = data;
        this.userApi.apiUsersIdDelete(e.userId).subscribe(() => {
          this.loading = false;
          this.toasterService.success("Record deleted successfully")
        },
          error => {
            this.loading = false;
          })
      }
      else {
        this.loading = false;
      }
    });
  }

  ShowEdit(element) {
    this.router.navigate(['edit-doctor/', element.userId], { relativeTo: this.route });
  }

}
