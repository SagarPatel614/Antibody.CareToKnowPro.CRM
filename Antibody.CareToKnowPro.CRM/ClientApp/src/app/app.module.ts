import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CookieService } from 'ngx-cookie-service';
import { ClipboardModule } from 'ngx-clipboard';
import { LoginComponent } from './Login/login.component';
import { AlertComponent } from './alert/alert.component';
import { DialogComponent } from './dialog/dialog.component';
import { ProgressComponent } from './progress/progress.component';
import { FooterComponent } from './footer/footer.component';

import { AuthGuard } from './service/auth-guard.service';
import { AuthService } from './service/auth.service';
import { ClipboardServiceModule } from './service/clipboard.service';
import { ApiModule } from './api/api.module';
import { appRoutes } from './routes';
import { MaterialModule } from './module/material.module';
import { UTCToLocalDatePipe } from './pipes/utcDate';
import { SplitOnUppercasePipe } from './pipes/SplitOnUppercase';
import { BasicAuthInterceptor } from './service/basic-auth.interceptor';
import { ErrorInterceptor } from './service/error.interceptor';

import { ComponentsModule } from './components/components.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { DoctorsComponent } from './doctor/doctors.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { NotificationService } from './service/notifications';
import { PasswordDialogComponent } from './dialog/passwordDialog.component';
import {MatRadioModule} from '@angular/material/radio';
import { AdminLayoutModule } from './layouts/admin-layout/admin-layout.module';
import { CommonModule } from '@angular/common';
import { ExportdDialogComponent } from './dialog/exportDialog.component';
import { SharedService } from './service/shared.service';
import { RecordCheckComponent } from './recordcheck/recordcheck.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    FooterComponent,
    HomeComponent,
    SplitOnUppercasePipe,
    LoginComponent,
    AlertComponent,
    ProgressComponent,
    DialogComponent,
    AdminLayoutComponent,
    PasswordDialogComponent,
    ExportdDialogComponent,
    RecordCheckComponent
  ],
  imports: [
    // BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserModule,
    BrowserAnimationsModule,
   // SharedModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ApiModule,
    ClipboardModule,
    MaterialModule,
    ToastrModule.forRoot({
      timeOut: 10000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),
    RouterModule.forRoot(appRoutes),
    ComponentsModule
  ],
  exports: [CommonModule],
  entryComponents: [
    DialogComponent,
    PasswordDialogComponent,
    ExportdDialogComponent
  ],
  providers: [AuthGuard, CookieService, ClipboardServiceModule, NotificationService, SharedService,
    //{ provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }


