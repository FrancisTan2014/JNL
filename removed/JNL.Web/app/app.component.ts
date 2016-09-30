import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { HeaderService } from './services/header.service';

@Component({
    selector: 'my-app',
    templateUrl: 'app/app.component.html',
    styleUrls: ['app/app.component.css', 'app/meterialize-effect.min.css']
})
export class AppComponent implements OnInit, AfterViewInit {
    url: any;
    static loaded: boolean;

    constructor(
        private headerService: HeaderService
    ){}

    showHeader() {
        return this.headerService.HeaderVisible;
    }

    loaderVisible() {
        return AppComponent.loaded;
    }

    ngOnInit(){
        
    }

    ngAfterViewInit(){
        setTimeout(function() {
            AppComponent.loaded = true;
        });
    }
}