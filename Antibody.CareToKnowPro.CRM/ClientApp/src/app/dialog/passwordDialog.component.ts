import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { ILoginProfile as LoginProfile } from '../model/models';
import { NotificationService } from '../service/notifications';
import { LoginProfilesService } from '../api/api';
import { Subscription, BehaviorSubject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-password-dialog',
  templateUrl: './passwordDialog.component.html',
  styleUrls: ['./passwordDialog.component.css']
})
export class PasswordDialogComponent implements OnInit {

  passwordForm: FormGroup;
  submitted = false;
  loading = false;
  currentUser: LoginProfile;
  currentUserSubscription: Subscription;
  private currentUserSubject: BehaviorSubject<LoginProfile>;

  constructor(private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<PasswordDialogComponent>,
    private toasterService: NotificationService,
    private loginProfileService: LoginProfilesService,
    @Inject(MAT_DIALOG_DATA) public data: any) {

      this.currentUserSubject = new BehaviorSubject<LoginProfile>(JSON.parse(localStorage.getItem('currentUser')));
      this.currentUser = this.currentUserSubject.value;
  }

  get f() { return this.passwordForm.controls; }

  ngOnInit() {
   this.passwordForm = this.formBuilder.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmNewPassword: ['', Validators.required]
    },{validator: this.passwordConfirming});
  }

  passwordConfirming(c: AbstractControl): { invalid: boolean } {
    if (c.get('newPassword').value !== c.get('confirmNewPassword').value) {
        return {invalid: true};
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    this.submitted = true;
    this.loading = true;
    if (this.passwordForm.invalid) {
      return;
    }

    var passwordModel ={
      oldPassword : this.f.currentPassword.value,
      newPassword : this.f.newPassword.value,
      confirmNewPassword: this.f.confirmNewPassword.value,
      loginProfileId: this.currentUser.loginProfileId
    }

    this.loginProfileService.apiLoginProfilesChangePassword(this.currentUser.loginProfileId, passwordModel).subscribe( user => {
      this.loading = false;
      localStorage.removeItem('currentUser');
      localStorage.setItem('currentUser', JSON.stringify(user));
      this.currentUser = user;
      this.currentUserSubject.next(user);
      this.toasterService.success("Password changed successfully");
      this.dialogRef.close();
    },
    error=> {
      this.loading = false;
    });
  }
}
