import { Component, OnInit, OnDestroy, Renderer, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { HeaderService } from './../../services/header.service';
import { LoginService } from './../../services/login.service';

import { User } from './../../model/user';

@Component({
    selector:'login',
    templateUrl:'app/view/login/login.html',
    styleUrls: [ 'app/view/login/login.css' ]
})
export class LoginComponent implements OnInit,OnDestroy,AfterViewInit {
    public loginUser: User;
    public msg: string;
    
    private backUrl: string;

    @ViewChild('workIdInput') workIdInput: ElementRef;

    constructor(
        private loginService: LoginService,
        private headerService: HeaderService,
        private renderer: Renderer,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.loginUser = new User('', '');
     }

    onInputChange(){
        this.msg = '';
    }

    onSubmit() {
        if (this.loginService.login(this.loginUser.workId + '', this.loginUser.password)) {
            this.msg = '登录成功';
            this.router.navigate(['/basic']);
        } else {
            this.msg = '用户名或者密码错误';
        }
    }

    ngOnInit() {
        this.headerService.HeaderVisible = false;
        this.backUrl = this.route.params['backUrl'];
    }

    ngOnDestroy() {
        this.headerService.HeaderVisible = true;
    }

    ngAfterViewInit(){
        this.renderer.invokeElementMethod(this.workIdInput.nativeElement, 'focus');
    }
}