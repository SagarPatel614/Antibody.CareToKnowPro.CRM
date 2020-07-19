import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams,
         HttpResponse, HttpEvent }                           from '@angular/common/http';
import { CustomHttpUrlEncodingCodec } from '../api/encoder';

import { Observable }                                        from 'rxjs';

import { ILoginProfile as LoginProfile, IPassword } from '../model/loginProfile';

import { BASE_PATH, COLLECTION_FORMATS } from '../api/variables';
import { Configuration } from '../api/configuration';


@Injectable({ providedIn: 'root' })
export class LoginProfilesService {

    protected basePath = '/';
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(protected httpClient: HttpClient,
       @Optional()@Inject('BASE_URL') basePath: string,
       @Optional() configuration: Configuration) {

        // @Optional() @Inject(BASE_PATH) basePath: string,
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
    public apiLoginProfilesGet(observe?: 'body', reportProgress?: boolean): Observable<Array<LoginProfile>>;
    public apiLoginProfilesGet(observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<LoginProfile>>>;
    public apiLoginProfilesGet(observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<LoginProfile>>>;
    public apiLoginProfilesGet(observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

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

        return this.httpClient.request<Array<LoginProfile>>('get',`${this.basePath}/api/LoginProfiles`,
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
    public apiLoginProfilesIdDelete(id: number, observe?: 'body', reportProgress?: boolean): Observable<LoginProfile>;
    public apiLoginProfilesIdDelete(id: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<LoginProfile>>;
    public apiLoginProfilesIdDelete(id: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<LoginProfile>>;
    public apiLoginProfilesIdDelete(id: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling apiLoginProfilesIdDelete.');
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

        return this.httpClient.request<LoginProfile>('delete',`${this.basePath}/api/LoginProfiles/${encodeURIComponent(String(id))}`,
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
    public apiLoginProfilesIdGet(id: number, observe?: 'body', reportProgress?: boolean): Observable<LoginProfile>;
    public apiLoginProfilesIdGet(id: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<LoginProfile>>;
    public apiLoginProfilesIdGet(id: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<LoginProfile>>;
    public apiLoginProfilesIdGet(id: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling apiLoginProfilesIdGet.');
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

        return this.httpClient.request<LoginProfile>('get',`${this.basePath}/api/LoginProfiles/${encodeURIComponent(String(id))}`,
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
    public apiLoginProfilesIdPut(id: number, body?: LoginProfile, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public apiLoginProfilesIdPut(id: number, body?: LoginProfile, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public apiLoginProfilesIdPut(id: number, body?: LoginProfile, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public apiLoginProfilesIdPut(id: number, body?: LoginProfile, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling apiLoginProfilesIdPut.');
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

        return this.httpClient.request<any>('put',`${this.basePath}/api/LoginProfiles/${encodeURIComponent(String(id))}`,
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
    public apiLoginProfilesPost(body?: LoginProfile, observe?: 'body', reportProgress?: boolean): Observable<LoginProfile>;
    public apiLoginProfilesPost(body?: LoginProfile, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<LoginProfile>>;
    public apiLoginProfilesPost(body?: LoginProfile, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<LoginProfile>>;
    public apiLoginProfilesPost(body?: LoginProfile, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


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

        return this.httpClient.request<LoginProfile>('post',`${this.basePath}/api/LoginProfiles`,
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
    public apiLoginProfilesChangePassword(id: number, body?: IPassword, observe?: 'body', reportProgress?: boolean): Observable<LoginProfile>;
    public apiLoginProfilesChangePassword(id: number, body?: IPassword, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<LoginProfile>>;
    public apiLoginProfilesChangePassword(id: number, body?: IPassword, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<LoginProfile>>;
    public apiLoginProfilesChangePassword(id: number, body?: IPassword, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


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

        return this.httpClient.request<IPassword>('put',`${this.basePath}/api/LoginProfiles/${encodeURIComponent(String(id))}/ChangePassword`,
            {
                body: body,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

}
