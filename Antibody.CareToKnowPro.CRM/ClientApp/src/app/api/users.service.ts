import { Inject, Injectable, Optional } from '@angular/core';
import {
  HttpClient, HttpHeaders, HttpParams,
  HttpResponse, HttpEvent
} from '@angular/common/http';
import { CustomHttpUrlEncodingCodec } from '../api/encoder';

import { Observable } from 'rxjs';

import { IUser as User, IRegister, IUserPagedResponse as UserPagedResponse } from '../model/user';

import { BASE_PATH, COLLECTION_FORMATS } from '../api/variables';
import { Configuration } from '../api/configuration';


@Injectable({ providedIn: 'root' })
export class UsersService {

  protected basePath = '/';
  public defaultHeaders = new HttpHeaders();
  public configuration = new Configuration();

  constructor(protected httpClient: HttpClient, @Optional() @Inject('BASE_URL') basePath: string, @Optional() configuration: Configuration) {
    if (basePath) {
      this.basePath = basePath.charAt(basePath.length - 1) === "/" ? basePath.substring(0, basePath.length - 1) : basePath;
    }
    if (configuration) {
      this.configuration = configuration;
      this.basePath = basePath || configuration.basePath || this.basePath;
    }
  }

  /**
   * @param consumes string[] mime-types
   * @return true: consumes contains 'multipart/form-data', false: otherwise
   */
  private canConsumeForm(consumes: string[]): boolean {
    const form = 'multipart/form-data';
    for (const consume of consumes) {
      if (form === consume) {
        return true;
      }
    }
    return false;
  }


  /**
   *
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public apiUsersGet(observe?: 'body', reportProgress?: boolean): Observable<Array<User>>;
  public apiUsersGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<User>>>;
  public apiUsersGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<User>>>;
  public apiUsersGet(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    let headers = this.defaultHeaders;

    // to determine the Accept header
    let httpHeaderAccepts: string[] = [
      'text/plain',
      'application/json',
      'text/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
    ];

    return this.httpClient.request<Array<User>>('get', `${this.basePath}/api/Users`,
      {
        withCredentials: this.configuration.withCredentials,
        headers: headers,
        observe: observe,
        reportProgress: reportProgress
      }
    );
  }

  public apiPagedUsers(body?: any, observe?: 'body', reportProgress?: boolean): Observable<UserPagedResponse>;
  public apiPagedUsers(body?: any, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<UserPagedResponse>>;
  public apiPagedUsers(body?: any, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<UserPagedResponse>>;
  public apiPagedUsers(body?: any, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    let headers = this.defaultHeaders;

    // to determine the Accept header
    let httpHeaderAccepts: string[] = [
      'text/plain',
      'application/json',
      'text/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
      'application/json-patch+json',
      'application/json',
      'text/json',
      'application/_*+json'
    ];

    const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
    if (httpContentTypeSelected != undefined) {
      headers = headers.set('Content-Type', httpContentTypeSelected);
    }

    return this.httpClient.request<Array<User>>('post', `${this.basePath}/api/Users/Paged`,
      {
        body: body,
        withCredentials: this.configuration.withCredentials,
        headers: headers,
        observe: observe,
        reportProgress: reportProgress
      }
    );
  }

  public apiPagedUsersForExport(body?: any, observe?: 'body', reportProgress?: boolean): Observable<any>;
  public apiPagedUsersForExport(body?: any, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
  public apiPagedUsersForExport(body?: any, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
  public apiPagedUsersForExport(body?: any, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    let headers = this.defaultHeaders;

    // to determine the Accept header
    let httpHeaderAccepts: string[] = [
      'text/plain',
      'application/json',
      'text/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
      'application/json-patch+json',
      'application/json',
      'text/json',
      'application/_*+json'
    ];

    const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
    if (httpContentTypeSelected != undefined) {
      headers = headers.set('Content-Type', httpContentTypeSelected);
    }

    return this.httpClient.request<any>('post', `${this.basePath}/api/Users/Export/${encodeURIComponent(Boolean(body.OnlyCurrentPage))}`,
      {
        body: body,
        withCredentials: this.configuration.withCredentials,
        headers: headers,
        observe: observe,
        reportProgress: reportProgress
      }
    );
  }

  public Getfilter(observe?: 'body', reportProgress?: boolean): Observable<any>;
  public Getfilter(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
  public Getfilter(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
  public Getfilter(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    let headers = this.defaultHeaders;

    // to determine the Accept header
    let httpHeaderAccepts: string[] = [
      'text/plain',
      'application/json',
      'text/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
    ];

    return this.httpClient.request<any>('get', `${this.basePath}/api/Users/filter`,
      {
        withCredentials: this.configuration.withCredentials,
        headers: headers,
        observe: observe,
        reportProgress: reportProgress
      }
    );
  }

  /**
   *
   *
   * @param id
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public apiUsersIdDelete(id: number, observe?: 'body', reportProgress?: boolean): Observable<User>;
  public apiUsersIdDelete(id: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<User>>;
  public apiUsersIdDelete(id: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<User>>;
  public apiUsersIdDelete(id: number, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (id === null || id === undefined) {
      throw new Error('Required parameter id was null or undefined when calling apiUsersIdDelete.');
    }

    let headers = this.defaultHeaders;

    // to determine the Accept header
    let httpHeaderAccepts: string[] = [
      'text/plain',
      'application/json',
      'text/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
    ];

    return this.httpClient.request<User>('delete', `${this.basePath}/api/Users/${encodeURIComponent(String(id))}`,
      {
        withCredentials: this.configuration.withCredentials,
        headers: headers,
        observe: observe,
        reportProgress: reportProgress
      }
    );
  }

  /**
   *
   *
   * @param id
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public apiUsersIdGet(id: number, observe?: 'body', reportProgress?: boolean): Observable<User>;
  public apiUsersIdGet(id: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<User>>;
  public apiUsersIdGet(id: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<User>>;
  public apiUsersIdGet(id: number, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (id === null || id === undefined) {
      throw new Error('Required parameter id was null or undefined when calling apiUsersIdGet.');
    }

    let headers = this.defaultHeaders;

    // to determine the Accept header
    let httpHeaderAccepts: string[] = [
      'text/plain',
      'application/json',
      'text/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
    ];

    return this.httpClient.request<User>('get', `${this.basePath}/api/Users/${encodeURIComponent(String(id))}`,
      {
        withCredentials: this.configuration.withCredentials,
        headers: headers,
        observe: observe,
        reportProgress: reportProgress
      }
    );
  }

  /**
   *
   *
   * @param id
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public apiUsersIdPut(id: number, body?: User, observe?: 'body', reportProgress?: boolean): Observable<any>;
  public apiUsersIdPut(id: number, body?: User, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
  public apiUsersIdPut(id: number, body?: User, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
  public apiUsersIdPut(id: number, body?: User, observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    if (id === null || id === undefined) {
      throw new Error('Required parameter id was null or undefined when calling apiUsersIdPut.');
    }


    let headers = this.defaultHeaders;

    // to determine the Accept header
    let httpHeaderAccepts: string[] = [
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
      'application/json-patch+json',
      'application/json',
      'text/json',
      'application/_*+json'
    ];
    const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
    if (httpContentTypeSelected != undefined) {
      headers = headers.set('Content-Type', httpContentTypeSelected);
    }

    return this.httpClient.request<any>('put', `${this.basePath}/api/Users/${encodeURIComponent(String(id))}`,
      {
        body: body,
        withCredentials: this.configuration.withCredentials,
        headers: headers,
        observe: observe,
        reportProgress: reportProgress
      }
    );
  }

  /**
   *
   *
   * @param body
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public apiUsersPost(body?: User, observe?: 'body', reportProgress?: boolean): Observable<User>;
  public apiUsersPost(body?: User, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<User>>;
  public apiUsersPost(body?: User, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<User>>;
  public apiUsersPost(body?: User, observe: any = 'body', reportProgress: boolean = false): Observable<any> {


    let headers = this.defaultHeaders;

    // to determine the Accept header
    let httpHeaderAccepts: string[] = [
      'text/plain',
      'application/json',
      'text/json'
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
      'application/json-patch+json',
      'application/json',
      'text/json',
      'application/_*+json'
    ];
    const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
    if (httpContentTypeSelected != undefined) {
      headers = headers.set('Content-Type', httpContentTypeSelected);
    }

    return this.httpClient.request<User>('post', `${this.basePath}/api/Users`,
      {
        body: body,
        withCredentials: this.configuration.withCredentials,
        headers: headers,
        observe: observe,
        reportProgress: reportProgress
      }
    );
  }

  /**
   *
   *
   * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
   * @param reportProgress flag to report request and response progress.
   */
  public apiUsersRegisterGet(observe?: 'body', reportProgress?: boolean): Observable<IRegister>;
  public apiUsersRegisterGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<IRegister>>;
  public apiUsersRegisterGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<IRegister>>;
  public apiUsersRegisterGet(observe: any = 'body', reportProgress: boolean = false): Observable<any> {

    let headers = this.defaultHeaders;

    // to determine the Accept header
    let httpHeaderAccepts: string[] = [
    ];
    const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
    if (httpHeaderAcceptSelected != undefined) {
      headers = headers.set('Accept', httpHeaderAcceptSelected);
    }

    // to determine the Content-Type header
    const consumes: string[] = [
    ];

    return this.httpClient.get<IRegister>(`${this.basePath}/api/Users/register`,
      {
        withCredentials: this.configuration.withCredentials,
        headers: headers,
        observe: observe,
        reportProgress: reportProgress
      }
    );
  }

}
