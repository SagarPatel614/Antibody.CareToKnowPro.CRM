import { Injectable, Inject, ErrorHandler, Injector } from '@angular/core';
import { HttpClient, HttpInterceptor, HttpRequest, HttpHandler, HttpUserEvent, HttpEvent, HttpHeaders, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from "rxjs";
import { map, catchError, throwIfEmpty, tap} from 'rxjs/operators';
import { Router } from '@angular/router';
import { RequestOptions } from '@angular/http';
import { CookieService } from 'ngx-cookie-service';
import { ToastrService } from 'ngx-toastr';
import { ILogin as Login, ILoginProfile as LoginProfile } from '../model/models';



@Injectable({ providedIn: 'root' })
export class AuthService  {
  private currentUserSubject: BehaviorSubject<LoginProfile>;
  public currentUser: Observable<LoginProfile>;

  rootUrl;
  readonly authcookie = 'Antibody.CareToKnowPro.CRM.Auth';

  constructor(private router: Router,
    private http: HttpClient,
    private cookieService: CookieService,
    @Inject('BASE_URL') baseUrl: string,
    private injector: Injector) {
    this.currentUserSubject = new BehaviorSubject<LoginProfile>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
    this.rootUrl = baseUrl;
  }

    public get currentUserValue(): LoginProfile {
      return this.currentUserSubject.value;
  }

    public get isAuthCookieExists(): boolean {
      return this.cookieService.check(this.authcookie);
  }

    login(username: string, password: string): Observable<Login> {
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    let options = { headers: headers };

    var data = JSON.stringify({ UserName:username, Password:password });

      return this.http.post<Login>(this.rootUrl + 'api/account/login', data, options)
      .pipe(map(login => {
          // login successful if there's a jwt token in the response
          if (login) {
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem('currentUser', JSON.stringify(login.user));
            this.currentUserSubject.next(login.user);
          }
          return login;
        }));
       // catchError(this.handleError));
        // catchError( (error: ModelError) => {
        //   if (error.Code === 400) {
        //     return throwError("Invalid credentials");
        //   }
        //   if (error.Code === 404) {
        //     return throwError("Not Found");
        //   }
        //   return throwError("Internal Server Error. Please contact IT.");
        // }));
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.cookieService.delete(this.authcookie);
    this.currentUserSubject.next(null);
  }
}
