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
var platform_browser_1 = require('@angular/platform-browser');
var forms_1 = require('@angular/forms');
var menu_1 = require('@angular2-material/menu');
var icon_1 = require('@angular2-material/icon');
var ng2_bootstrap_1 = require('ng2-bootstrap/ng2-bootstrap');
var header_service_1 = require('./services/header.service');
var login_service_1 = require('./services/login.service');
var app_router_1 = require('./app.router');
var basic_files_component_1 = require('./view/basic-files/basic-files.component');
var login_component_1 = require('./view/login/login.component');
var header_component_1 = require('./view/layout/header.component');
var page_notfound_component_1 = require('./view/errors/page-notfound.component');
// import { SpinnerComponent } from './view/spinner/spinner.component';
var app_component_1 = require('./app.component');
var AppModule = (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule, forms_1.FormsModule, ng2_bootstrap_1.Ng2BootstrapModule,
                menu_1.MdMenuModule.forRoot(), icon_1.MdIconModule.forRoot(),
                app_router_1.router],
            declarations: [app_component_1.AppComponent, login_component_1.LoginComponent, header_component_1.HeaderComponent, basic_files_component_1.BasicFilesComponent, page_notfound_component_1.PageNotfoundComponent],
            providers: [header_service_1.HeaderService, login_service_1.LoginService],
            bootstrap: [app_component_1.AppComponent]
        }), 
        __metadata('design:paramtypes', [])
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map