import { Component, OnInit } from '@angular/core';
import { ILoginProfile as LoginProfile } from '../model/models';
import { Subscription, BehaviorSubject, of } from 'rxjs';
import { AuthService } from '../service/services';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NotificationService } from '../service/notifications';
import { LoginProfilesService } from '../api/api';
import { error } from 'protractor';
import { PasswordDialogComponent } from '../dialog/passwordDialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {


  currentUser: LoginProfile;
  currentUserSubscription: Subscription;
  private currentUserSubject: BehaviorSubject<LoginProfile>;
  profileForm: FormGroup;
  loading = false;
  loading1 = false;
  submitted = false;
  provinces = [];

  constructor(
    private authenticationService: AuthService,
    private loginProfileService: LoginProfilesService,
    private formBuilder: FormBuilder,
    private toasterService: NotificationService,
    public dialog: MatDialog
  ) {
    this.currentUserSubject = new BehaviorSubject<LoginProfile>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.value;
    this.provinces = this.getProvinces();
    // this.currentUserSubscription = this.authenticationService.currentUser.subscribe(user => {
    //   this.currentUser = user;
    // });
  }

  getProvinces() {
    return [
      { id: 'AB', name: 'Alberta' },
      { id: 'BC', name: 'British Columbia' },
      { id: 'MB', name: 'Manitoba' },
      { id: 'NB', name: 'New Brunswick' },
      { id: 'NF', name: 'Newfoundland' },
      { id: 'NT', name: 'Northwest Territories' },
      { id: 'NS', name: 'Nova Scotia' },
      { id: 'NU', name: 'Nunavut' },
      { id: 'ON', name: 'Ontario' },
      { id: 'PE', name: 'Prince Edward Island' },
      { id: 'QC', name: 'Quebec' },
      { id: 'SK', name: 'Saskatchewan' },
      { id: 'YT', name: 'Yukon Territory' }
    ];
  }

  ngOnInit(): void {
    this.profileForm = this.formBuilder.group({
      companyName: [this.currentUser.companyName, [Validators.required]],
      userName: [this.currentUser.userName, [Validators.required, Validators.maxLength(25)]],
     // password: ['', Validators.required],
      email: [this.currentUser.email, [Validators.required, Validators.email]],
      firstName: [this.currentUser.firstName, [Validators.required, Validators.maxLength(25)]],
      postal: [this.currentUser.postal],
      lastName: [this.currentUser.lastName, [Validators.required, Validators.maxLength(25)]],
      street1: [this.currentUser.street1],
      city: [this.currentUser.city],
      provCode: [this.currentUser.provCode],
      country: [this.currentUser.country],
      notes:[this.currentUser.notes],
      phoneNumber:[this.currentUser.phoneNumber]
    });

  }

  getError(){
    console.log(this.profileForm?.controls.userName?.errors);
  }
  get f() { return this.profileForm.controls; }

  onSubmit() {
    this.submitted = true;
    this.toasterService.clear();

    // stop here if form is invalid
    if (this.profileForm.invalid) {
     return;
    }

    var loginprofile = {
      userName : this.f.userName.value,
      password : this.currentUser.passwordHash,
      companyName : this.f.companyName.value,
      provCode : this.f.provCode.value,
      country : this.f.country.value,
      street1 : this.f.street1.value,
      notes : this.f.notes.value,
      firstName : this.f.firstName.value,
      lastName : this.f.lastName.value,
      city : this.f.city.value,
      postal : this.f.postal.value,
      phoneNumber : this.f.phoneNumber.value,
      email : this.f.email.value,
      loginProfileId : this.currentUser.loginProfileId,
      emailConfirmed: true,
      passwordHash: this.currentUser.passwordHash,
      lockoutEndDateUtc: this.currentUser.lockoutEndDateUtc,
      lockoutEnabled: this.currentUser.lockoutEnabled,
      accessFailedCount: this.currentUser.accessFailedCount,
      status: this.currentUser.status,
      profileQuestion: this.currentUser.profileQuestion,
      profileAnswer: this.currentUser.profileAnswer
    }

    this.loading = true;
    this.loginProfileService.apiLoginProfilesIdPut(this.currentUser.loginProfileId, loginprofile).subscribe( () => {
      this.loading = false;

      localStorage.removeItem('currentUser');
      localStorage.setItem('currentUser', JSON.stringify(loginprofile));
      this.currentUser = loginprofile;
      this.currentUserSubject.next(loginprofile);
      this.toasterService.success("Profile updated successfully");
    },
    error=> {
      this.loading = false;
    });
  }

  openDialogForChangePassword(): void {
    this.loading1 = true;
    const dialogRef = this.dialog.open(PasswordDialogComponent, {
      width: '500px',
      data: {  }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.loading1 = false;
      if (result) {
        //this.toasterService.success("");
      }
    });
  }

  ngOnDestroy() {
    // unsubscribe to ensure no memory leaks
   // this.currentUserSubscription.unsubscribe();
  }
}
