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
var User_1 = require('./../model/User');
var LoginService = (function () {
    function LoginService() {
        this.testUser = new User_1.User('2008441101', 'prutgh990610cqdj', 'FrancisTan', 1, '集宁机务段');
    }
    Object.defineProperty(LoginService.prototype, "CurrentUser", {
        get: function () {
            return this.currentUser;
        },
        enumerable: true,
        configurable: true
    });
    LoginService.prototype.login = function (workId, password) {
        console.log("password equals: " + (password === this.testUser.password));
        if (workId === this.testUser.workId && password === this.testUser.password) {
            this.currentUser = this.testUser;
            return true;
        }
        return false;
    };
    LoginService.prototype.logout = function () {
        this.currentUser = null;
    };
    LoginService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], LoginService);
    return LoginService;
}());
exports.LoginService = LoginService;
//# sourceMappingURL=login.service.js.map