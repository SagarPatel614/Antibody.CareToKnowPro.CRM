import { Injectable } from '@angular/core';
import { HttpClient, HttpInterceptor, HttpRequest, HttpHandler, HttpUserEvent, HttpEvent, HttpHeaders, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../service/auth.service';

@Injectable()
export class BasicAuthInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with jwt token if available
        // not using jwt
        // let currentUser = this.authenticationService.currentUserValue;
        // if (currentUser && currentUser.) {
        //     request = request.clone({
        //         setHeaders: {
        //             Authorization: `Bearer ${currentUser.token}`
        //         }
        //     });
        // }

        return next.handle(request);
    }
}
