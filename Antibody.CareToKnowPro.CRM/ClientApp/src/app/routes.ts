import { Routes } from '@angular/router'
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './service/auth-guard.service'
import { LoginComponent } from './Login/login.component'
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';

export const appRoutes: Routes = [
  // { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'Home', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'home', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
  // {
  //   path: 'login', component: LoginComponent,
  //   children: [{ path: '', component: LoginComponent }]
  // },

  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: '',
    component: AdminLayoutComponent,
    children: [{
      path: '',
      loadChildren: './layouts/admin-layout/admin-layout.module#AdminLayoutModule'
    }]
  }
];
