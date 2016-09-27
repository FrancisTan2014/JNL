"use strict";
var http_1 = require('@angular/http');
var Observable_1 = require('rxjs/Observable');
require('rxjs/Rx');
var HttpHelpers = (function () {
    function HttpHelpers(_http) {
        this._http = _http;
    }
    Object.defineProperty(HttpHelpers.prototype, "haserror", {
        get: function () {
            return this.errormsg != null;
        },
        enumerable: true,
        configurable: true
    });
    HttpHelpers.prototype.getaction = function (path) {
        return this._http.get(path)
            .map(function (res) {
            return res.json();
        })
            .catch(this._handleError);
    };
    HttpHelpers.prototype.postaction = function (param, path) {
        var _this = this;
        this.errormsg = null;
        var body = JSON.stringify(param);
        var headers = new http_1.Headers({ 'Content-Type': 'application/json' });
        var options = new http_1.RequestOptions({ headers: headers });
        return this._http.post(path, body, options)
            .map(function (m) {
            var jsonresult = m.json();
            if (jsonresult.haserror) {
                _this.errormsg = jsonresult.errormessage;
            }
            return jsonresult;
        })
            .catch(this._handleError);
    };
    HttpHelpers.prototype._handleError = function (error) {
        return Observable_1.Observable.throw(error.text() || 'Server error');
    };
    return HttpHelpers;
}());
exports.HttpHelpers = HttpHelpers;
//# sourceMappingURL=http-helper.js.map