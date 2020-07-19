import { Component, OnInit, ViewChild, NgZone, AfterViewInit, QueryList, ViewChildren } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Subject } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { MatChipInputEvent } from '@angular/material/chips';
import { IUser as User, IRegister as Register } from '../model/user';
import { UsersService } from '../api/api';
import { IUserSpecialty as UserSpecialty, IEvent as Event, IEventEntityProperty as EventEntityProperty, IEventEntity as EventEntity } from '../model/models';
import { groupBy } from 'rxjs/operators';
import { group, trigger, style, state, transition, animate } from '@angular/animations';
import { NotificationService } from '../service/notifications';
import { error } from 'protractor';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { Metrics, Message } from '../model/message';

@Component({
  selector: 'edit-doctor',
  templateUrl: './edit-doctors.component.html',
  styleUrls: ['./edit-doctors.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
    ])
  ]
})
export class EditDoctorComponent implements OnInit, AfterViewInit {

  userForm: FormGroup;
  provinces = [];
  years = [];
  languages: any;
  emailStatusList: any;
  statusList: any
  loading = false;
  isDatafilled = false;
  user: User;

  userTransaction: MatTableDataSource<EventEntity>;
  expandedElement: EventEntity | null;
  @ViewChild(MatSort, {static: false}) sort: MatSort;
  // @ViewChild(MatSort, {static: false}) set content(sort: MatSort) {
  //   if(!this.userTransaction){
  //     this.userTransaction = new MatTableDataSource<Event>();
  //   }
  //   this.userTransaction.sort = sort;
  // }
  events: Array<EventEntity>;
  transactions: Array<EventEntityProperty>;
  trasactionProperty: MatTableDataSource<EventEntityProperty>;


  displayedColumns: string[] = ['eventType', 'user', 'date', 'note'];
  isEventfilled = false;

  displayedPropertyColumns: string[] = ['propertyName', 'oldValue', 'newValue'];


  displayedMsgColumns: string[] = ['created', 'subject', 'campaign', 'lastAction', 'failureMessage'];
  displayedMatricsColumns: string[] = ['name', 'eventDateTime'];

  messages: MatTableDataSource<Message>;
  expandedMsgElement: Message | null;
  @ViewChild(MatSort, {static: false}) sort1: MatSort;



  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private ngZone: NgZone,
    private actRoute: ActivatedRoute,
    private userApi: UsersService,
    private toasterService: NotificationService,
    public dialog: MatDialog
  ) {
    var id = this.actRoute.snapshot.paramMap.get('id');
    this.languages = this.getLanguages();
    this.languages = this.getLanguages();
    this.emailStatusList = this.getEmailStatusList();
    this.statusList = this.getStatusList();
    userApi.apiUsersIdGet(+id).subscribe(data => {
      this.user = data;

      setTimeout(() => {
        this.isEventfilled = true;
        this.events = this.user.eventEntity.reverse();
        this.userTransaction = new MatTableDataSource<EventEntity>(this.events);
        this.userTransaction.sort = this.sort;
      }, 0);

      this.messages = new MatTableDataSource<Message>(this.user.messages);

      this.isDatafilled = true;
      var regex = /^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$/;
      var multipleEmailsRegex = /^(\s?[^\s;]+@[^\s;]+\.[^\s;]+\s?;)*(\s?[^\s;]+@[^\s;]+\.[^\s;]+)$/;
      this.userForm = this.formBuilder.group({
        UserId: [this.user.userId, [Validators.required]],
        Firstname: [this.user.firstName, [Validators.required, Validators.maxLength(200)]],
        Lastname: [this.user.lastName, [Validators.required, Validators.maxLength(200)]],
        Email: [this.user.email, [Validators.required, Validators.email, Validators.maxLength(200)]],
        SecondaryEmails:[this.user.secondaryEmails, [Validators.pattern(multipleEmailsRegex)]],
        SpecialtyIds: [this.user.userSpecialty.map(a => a.speciality?.specialtyId?.toString())],
        GraduationYear: [this.user.graduationYear?.toString()],
        ProvinceId: [this.user.provinceId.toString(), [Validators.required]],
        PreferredLanguage: [this.user.preferredLanguage, [Validators.required]],
        EmailStatus: [this.user.emailStatus, [Validators.required]],
        Status: [this.user.status, [Validators.required]],
        CreatedBy: [this.user.createdBy],
        DateCreated: [this.user.dateCreated],
        DateModified: [this.user.dateModified],
        ModifiedBy: [this.user.modifiedBy],
        Street1: [this.user.street1, Validators.maxLength(200)],
        City: [this.user.city, Validators.maxLength(200)],
        Postal: [this.user.postal, [Validators.maxLength(7), Validators.pattern(regex)]],
        Country: [this.user.country],
        PhoneNumber: [this.user.phoneNumber, Validators.maxLength(50)],
        Fax: [this.user.fax, Validators.maxLength(50)],
        AdditionalInfo: [this.user.additionalInfo],
        Notes: [this.user.notes],
        Other: [this.user.other],
      })
    });
  }
  @ViewChildren('innerTables') innerTables: QueryList<MatTable<EventEntityProperty>>;

  toggleRow(element: EventEntity) {
    element.eventEntityProperty && element.eventEntityProperty.length ? (this.expandedElement = this.expandedElement === element ? null : element) : null;
    //this.cd.detectChanges();
}

toggleRow1(element: Message) {
  element.metrics.events && element.metrics.events.length ? (this.expandedMsgElement = this.expandedMsgElement === element ? null : element) : null;
  //this.cd.detectChanges();
}

  ngAfterViewInit(): void {

  }

  get f() { return this.userForm.controls; }

  ngOnInit() {
    // this.userTransaction = new MatTableDataSource<Event>();
    // this.sort = new MatSort();
  }

  GetEventTypeDesc(typeId: number){
    switch(typeId){
      case 0: return "Record Created"; break;
      case 1: return "Record Updated"; break;
      case 2: return "Record Deleted"; break;
      default :{
        return "Type not found";
        break;
      }
    }
  }


  getLanguages() {
    return [
      { id: 'EN', name: 'English' },
      { id: 'FR', name: 'French' }
    ];
  }

  getEmailStatusList() {
    return [
      { id: "Subscribed", name: 'Subscribed' },
      { id: "Unsubscribed", name: 'Unsubscribed' }
      // { id: "Unverified", name: 'Unverified' }
    ];
  }

  getStatusList() {
    return [
      { id: "Practicing", name: 'Practicing' },
      { id: "Retired", name: 'Retired' },
      { id: "Moved", name: 'Moved' }
    ];
  }

  updateUserForm() {
    if (this.userForm.valid) {
      this.loading = true;

      let id = this.actRoute.snapshot.paramMap.get('id');
      this.userApi.apiUsersIdPut(+id, this.userForm.value).subscribe(res => {
        this.loading = false;
        this.toasterService.success("Record updated successfully")
        this.ngZone.run(() => this.router.navigateByUrl('/dashboard'))
      }, error => {
        this.loading = false;
      });

      // const dialogRef = this.dialog.open(DialogComponent, {
      //   width: '500px',
      //   data: { message: "Are you sure you want to update?" }
      // });
      // dialogRef.afterClosed().subscribe(result => {
      //   if (result) {

      //     let id = this.actRoute.snapshot.paramMap.get('id');
      //     this.userApi.apiUsersIdPut(+id, this.userForm.value).subscribe(res => {
      //       this.loading = false;
      //       this.toasterService.success("Record updated successfully")
      //       this.ngZone.run(() => this.router.navigateByUrl('/dashboard'))
      //     }, error => {
      //       this.loading = false;
      //     });
      //   }
      //   else {
      //     this.loading = false;
      //   }
      // });
    }
    else {
      this.loading = false;
    }
  }
}
