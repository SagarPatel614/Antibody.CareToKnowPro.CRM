import { Routes } from '@angular/router';

import { DashboardComponent } from '../../dashboard/dashboard.component';
import { UserProfileComponent } from '../../user-profile/user-profile.component';
import { DoctorsComponent } from '../../doctor/doctors.component';
import { LoginComponent } from 'src/app/Login/login.component';
import { AuthGuard } from 'src/app/service/auth-guard.service';
import { EditDoctorComponent } from 'src/app/doctor-edit/edit-doctors.component';
import { RecordCheckComponent } from 'src/app/recordcheck/recordcheck.component';

export const AdminLayoutRoutes: Routes = [
    { path: 'dashboard',      component: DashboardComponent,
                              children: [
                                { path: 'edit-doctor/:id', component: EditDoctorComponent }
                              ],
                              canActivate: [AuthGuard] },
    { path: 'user-profile',   component: UserProfileComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'doctor',         component: DoctorsComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'edit-doctor/:id', component: EditDoctorComponent, canActivate: [AuthGuard] },
    { path: 'recordcheck',   component: RecordCheckComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    { path: 'login',         component: LoginComponent, children: [{ path: '', component: LoginComponent }] }
];
