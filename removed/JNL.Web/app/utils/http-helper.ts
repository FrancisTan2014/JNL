import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/Rx';

export class HttpHelpers {
    constructor(private _http: Http) {
    }

    get haserror(): boolean {
        return this.errormsg != null;
    }

    errormsg: string;

    getaction<T>(path: string) {
        return this._http.get(path)
            .map(res => {
                return <T>res.json();
            })
            .catch(this._handleError);
    }

    postaction<T>(param: T | any, path: string) {
        this.errormsg = null;

        let body = JSON.stringify(param);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this._http.post(path, body, options)
            .map(m => {
                var jsonresult = <Models.ViewModel.JSONReturnVM<T>>m.json();

                if (jsonresult.haserror) {
                    this.errormsg = jsonresult.errormessage;
                }

                return jsonresult;
            })
            .catch(this._handleError);
    }

    private _handleError(error: Response) {
        return Observable.throw(error.text() || 'Server error');
    }
}