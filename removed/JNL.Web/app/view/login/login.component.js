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
var login_service_1 = require('./../../services/login.service');
var user_1 = require('./../../model/user');
var LoginComponent = (function () {
    function LoginComponent(loginService, headerService, renderer, router, route) {
        this.loginService = loginService;
        this.headerService = headerService;
        this.renderer = renderer;
        this.router = router;
        this.route = route;
        this.loginUser = new user_1.User('', '');
    }
    LoginComponent.prototype.onInputChange = function () {
        this.msg = '';
    };
    LoginComponent.prototype.onSubmit = function () {
        if (this.loginService.login(this.loginUser.workId + '', this.loginUser.password)) {
            this.msg = '登录成功';
            this.router.navigate(['/basic']);
        }
        else {
            this.msg = '用户名或者密码错误';
        }
    };
    LoginComponent.prototype.ngOnInit = function () {
        this.headerService.HeaderVisible = false;
        this.backUrl = this.route.params['backUrl'];
    };
    LoginComponent.prototype.ngOnDestroy = function () {
        this.headerService.HeaderVisible = true;
    };
    LoginComponent.prototype.ngAfterViewInit = function () {
        this.renderer.invokeElementMethod(this.workIdInput.nativeElement, 'focus');
    };
    __decorate([
        core_1.ViewChild('workIdInput'), 
        __metadata('design:type', core_1.ElementRef)
    ], LoginComponent.prototype, "workIdInput", void 0);
    LoginComponent = __decorate([
        core_1.Component({
            selector: 'login',
            templateUrl: 'app/view/login/login.html',
            styleUrls: ['app/view/login/login.css']
        }), 
        __metadata('design:paramtypes', [login_service_1.LoginService, header_service_1.HeaderService, core_1.Renderer, router_1.Router, router_1.ActivatedRoute])
    ], LoginComponent);
    return LoginComponent;
}());
exports.LoginComponent = LoginComponent;
//# sourceMappingURL=login.component.js.map