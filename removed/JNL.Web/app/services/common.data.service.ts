import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import {Observable} from 'rxjs/Observable';

import { HttpHelpers } from './../utils/http-helper';

import { WebConfig } from './../utils/web-config';

export class CommonDataService extends HttpHelpers {
    constructor(private http: Http) {
        super(http);
    }

    public getData<T>(params: Models.Params.GetListParams) {
        return this.postaction<T>(params, WebConfig.CommonAjaxUrl);
    }

    public postData<T>(param: T) {
        return this.postaction<T>(param, WebConfig.CommonAjaxUrl);
    }
}