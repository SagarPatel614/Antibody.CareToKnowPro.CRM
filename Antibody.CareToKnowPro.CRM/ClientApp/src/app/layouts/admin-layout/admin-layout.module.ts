import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminLayoutRoutes } from './admin-layout.routing';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import { UserProfileComponent } from '../../user-profile/user-profile.component';
import { DoctorsComponent } from '../../doctor/doctors.component';
import { MaterialModule } from 'src/app/module/material.module';
import { EditDoctorComponent } from 'src/app/doctor-edit/edit-doctors.component';
import { UTCToLocalDatePipe } from 'src/app/pipes/utcDate';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    ReactiveFormsModule,
    MaterialModule
  ],
  declarations: [
    UTCToLocalDatePipe,
    DashboardComponent,
    UserProfileComponent,
    DoctorsComponent,
    EditDoctorComponent
  ]
})

export class AdminLayoutModule { }
