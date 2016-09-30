"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var http_helper_1 = require('./../utils/http-helper');
var web_config_1 = require('./../utils/web-config');
var CommonDataService = (function (_super) {
    __extends(CommonDataService, _super);
    function CommonDataService(http) {
        _super.call(this, http);
        this.http = http;
    }
    CommonDataService.prototype.getData = function (params) {
        return this.postaction(params, web_config_1.WebConfig.CommonAjaxUrl);
    };
    CommonDataService.prototype.postData = function (param) {
        return this.postaction(param, web_config_1.WebConfig.CommonAjaxUrl);
    };
    return CommonDataService;
}(http_helper_1.HttpHelpers));
exports.CommonDataService = CommonDataService;
//# sourceMappingURL=common.data.service.js.map