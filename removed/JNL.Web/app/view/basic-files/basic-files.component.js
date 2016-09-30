"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var router_1 = require('@angular/router');
var header_service_1 = require('./../../services/header.service');
var mock_1 = require('./../../mock/mock');
require('rxjs/Rx');
var BasicFilesComponent = (function () {
    function BasicFilesComponent(route, router, headerService) {
        this.route = route;
        this.router = router;
        this.headerService = headerService;
        this.fileTypes = ['技术规章', '企业标准', '制度措施'];
        this.levels = ['总公司', '铁路局', '机务段'];
        this.files = mock_1.MockData.files;
        this.isLoading = false;
    }
    BasicFilesComponent.prototype.update = function () {
        console.log('method update invoked');
    };
    BasicFilesComponent.prototype.loadData = function () {
        this.isLoading = true;
        console.log(this.isLoading);
        var _this = this;
        window.setTimeout(function () {
            _this.files = [{
                    Id: '1',
                    FileName: 'Test',
                    FileNumber: '2008441101',
                    AddTime: '2016-09-27'
                }];
            _this.isLoading = false;
        }, 2000);
    };
    BasicFilesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.route.params.forEach(function (params) {
            var fileType = +params['fileType'];
            var level = +params['level'];
            if (fileType < 1 || fileType > 3 || level < 1 || level > 3) {
                _this.router.navigate(['/notfound']);
            }
            else {
                _this.title = _this.fileTypes[fileType - 1] + " - " + _this.levels[level - 1];
            }
        });
    };
    BasicFilesComponent = __decorate([
        core_1.Component({
            selector: 'basic-files',
            templateUrl: 'app/view/basic-files/basic-files.component.html'
        }), 
        __metadata('design:paramtypes', [router_1.ActivatedRoute, router_1.Router, header_service_1.HeaderService])
    ], BasicFilesComponent);
    return BasicFilesComponent;
}());
exports.BasicFilesComponent = BasicFilesComponent;
//# sourceMappingURL=basic-files.component.js.map