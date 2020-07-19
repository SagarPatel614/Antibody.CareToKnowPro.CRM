import { Injectable, ErrorHandler, Injector } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AuthService } from '../service/auth.service';
import { ToastrService, ActiveToast } from 'ngx-toastr';
import { IModelError as ModelError } from '../model/modelError';
import { NotificationService } from './notifications';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor, ErrorHandler {
    constructor(private authenticationService: AuthService,
                private injector: Injector) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      return next.handle(request).pipe(catchError(err => {
        if (err.status === 401) {
          // auto logout if 401 response returned from api
          this.authenticationService.logout();
          location.reload(true);
        } else {
          this.handleError(err);
        }

        const error = err.error.message || err.statusText;
        return throwError(error);
      }));
    }

    // generalize error handling any http call
  handleError(error: Error | HttpErrorResponse) {
    const notificationService = this.injector.get(NotificationService);
    if (error instanceof HttpErrorResponse) {
      // Server or connection error happened
      if (!navigator.onLine) {
        // Handle offline error
        return notificationService.error('No Internet Connection');
      } else {
        // Handle Http Error (error.status === 403, 404...)
        var err = error.error.error as ModelError
        if(err){
          //return notificationService.error(`${err.code} - ${err.message}`);
          return notificationService.error(`${err.message}`);
        }
        //return notificationService.error(`${error.status} - ${error.message}`);
        return notificationService.error(`${err.message}`);
      }
    } else {
      // Handle Client Error (Angular Error, ReferenceError...)
      return notificationService.error(error.toString());
    }
  }

    // intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    //   return next.handle(request).pipe(tap(() => { },
    //     (err: any) => {
    //       if (err instanceof HttpErrorResponse) {
    //         if (err.status !== 401) {
    //          // this.handleError(err);
    //           return;
    //         }
    //         localStorage.removeItem('currentUser');
    //         this.cookieService.delete(this.authcookie);
    //         this.currentUserSubject.next(null);
    //         //todo set return url as some meaning full path from this request has been made
    //        // this.router.navigate(['/login'], { queryParams: { returnUrl: request.url } });
    //        this.router.navigate(['/login']);
    //       }
    //     }));
    // }

     // generalize error handling any http call
  // handleError(error: Error | HttpErrorResponse) {
  //   const notificationService = this.injector.get(ToastrService);
  //   if (error instanceof HttpErrorResponse) {
  //     // Server or connection error happened
  //     if (!navigator.onLine) {
  //       // Handle offline error
  //       return notificationService.error('No Internet Connection');
  //     } else {
  //       // Handle Http Error (error.status === 403, 404...)
  //       return notificationService.error(`${error.status} - ${error.message}`);
  //     }
  //   } else {
  //     // Handle Client Error (Angular Error, ReferenceError...)
  //     return notificationService.error(error.toString());
  //   }
  // }
}
