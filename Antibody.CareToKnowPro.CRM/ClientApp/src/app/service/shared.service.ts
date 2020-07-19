import { Injectable, Inject, ErrorHandler, Injector } from '@angular/core';
import { HttpClient, HttpInterceptor, HttpRequest, HttpHandler, HttpUserEvent, HttpEvent, HttpHeaders, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from "rxjs";
import { map, catchError, throwIfEmpty, tap} from 'rxjs/operators';
import { Router } from '@angular/router';
import { RequestOptions } from '@angular/http';
import { CookieService } from 'ngx-cookie-service';
import { ToastrService } from 'ngx-toastr';
import { ILogin as Login, ILoginProfile as LoginProfile } from '../model/models';
import { FormBuilder } from '@angular/forms';



@Injectable({ providedIn: 'root' })
export class SharedService  {
  private filterSubject: BehaviorSubject<any>;
  public filter: Observable<any>;

  private formSubject: BehaviorSubject<any>;
  public form: Observable<any>;

  constructor(private router: Router,
    private http: HttpClient,
    private cookieService: CookieService,
    private formBuilder: FormBuilder,
    private injector: Injector) {

    this.filterSubject = new BehaviorSubject<any>({
      FirstName: '',
      LastName: '',
      Email: '',
      ProvinceIds: [],
      GraduationYear: [],
      PreferredLanguage: [],
      EmailStatus: [],
      Status: [],
      OtherSpecialties: [],
      SpecialtyIds: ['']
    });
    this.filter = this.filterSubject.asObservable();

    this.formSubject = new BehaviorSubject<any>(this.formBuilder.group({
      FirstName: [''],
      LastName: [''],
      Email: [''],
      ProvinceIds: [],
      GraduationYear: [],
      PreferredLanguage: [],
      EmailStatus: [],
      Status: [],
      OtherSpecialties: [],
      SpecialtyIds: ['']
    }));
    this.form = this.formSubject.asObservable();

  }

  public get currentfilterValue(): any {
      return this.filterSubject.value;
  }

  SetFilter(filter: any) {
    this.filterSubject.next(filter)
  }

  public get currentformValue(): any {
    return this.formSubject.value;
  }

 SetForm(form: any) {
  this.formSubject.next(form)
 }

}


