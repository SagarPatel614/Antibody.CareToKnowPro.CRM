import { Component, OnInit, ViewChild, NgZone } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Subject } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { MatChipInputEvent } from '@angular/material/chips';
import { IUser as User, IRegister as Register } from '../model/user';
import { UsersService } from '../api/api';
import { IUserSpecialty as UserSpecialty } from '../model/models';
import { groupBy } from 'rxjs/operators';
import { group } from '@angular/animations';
import { NotificationService } from '../service/notifications';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-doctors',
  templateUrl: './doctors.component.html',
  styleUrls: ['./doctors.component.css']
})
export class DoctorsComponent implements OnInit {

  userForm: FormGroup;
  provinces = [];
  years = [];
  languages: any;
  emailStatusList: any;
  statusList: any
  loading = false;
  isDatafilled = false;
  register: Register;

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
    this.emailStatusList = this.getEmailStatusList();
    this.statusList = this.getStatusList();
    userApi.apiUsersRegisterGet().subscribe(data => {
      this.register = data;
      this.isDatafilled = true;
    });
  };

  get f() { return this.userForm.controls; }

  ngOnInit() {
    var regex = /^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$/;
    var multipleEmailsRegex = /^(\s?[^\s;]+@[^\s;]+\.[^\s;]+\s?;)*(\s?[^\s;]+@[^\s;]+\.[^\s;]+)$/;
    this.userForm = this.formBuilder.group({
      Firstname: ['', [Validators.required, Validators.maxLength(200)]],
      Lastname: ['', [Validators.required, Validators.maxLength(200)]],
      Email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
      SecondaryEmails:['', [Validators.pattern(multipleEmailsRegex)]],
      SpecialtyIds: [''],
      GraduationYear: [''],
      ProvinceId: ['', [Validators.required]],
      PreferredLanguage: ['', [Validators.required]],
      EmailStatus: ['', [Validators.required]],
      Status: ['', [Validators.required]],
      CreatedBy: [''],
      DateCreated: [''],
      DateModified: [''],
      ModifiedBy: [''],
      Street1: ['', Validators.maxLength(200)],
      City: ['', Validators.maxLength(200)],
      Postal: ['', Validators.pattern(regex)],
      Country: ['Canada'],
      PhoneNumber: ['', Validators.maxLength(50)],
      Fax: ['', Validators.maxLength(50)],
      AdditionalInfo: [''],
      Notes: [''],
      Other: [''],

    })
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


  /* Submit book */
  submitUserForm() {
    this.loading = true;
    if (this.userForm.valid) {
      this.userApi.apiUsersPost(this.userForm.value).subscribe(res => {
        this.loading = false;
        this.toasterService.success("Record added successfully")
        this.ngZone.run(() => this.router.navigateByUrl('/dashboard'))
      }, error => {
        this.loading = false;
      });
    }
    else {
      this.loading = false;
    }
  }
}
