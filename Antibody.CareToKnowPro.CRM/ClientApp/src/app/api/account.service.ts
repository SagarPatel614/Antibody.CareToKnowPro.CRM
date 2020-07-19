import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams,
         HttpResponse, HttpEvent }                           from '@angular/common/http';
import { CustomHttpUrlEncodingCodec } from '../api/encoder';

import { Observable }                                        from 'rxjs';

import { ILogin as CommandResponse } from '../model/commandResponse';
import { IProblemDetails as ProblemDetails } from '../model/problemDetails';
import { IValidationProblemDetails as ValidationProblemDetails } from '../model/validationProblemDetails';

import { BASE_PATH, COLLECTION_FORMATS } from '../api/variables'
import { Configuration }                                     from '../api/configuration';


@Injectable()
export class AccountService {

    protected basePath = '/';
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

  constructor(protected httpClient: HttpClient, @Optional() @Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration) {
        if (basePath) {
            this.basePath = basePath;
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
}
