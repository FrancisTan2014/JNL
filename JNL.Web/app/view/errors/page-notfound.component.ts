import { Component, OnInit } from '@angular/core';

import { HeaderService } from './../../services/header.service';

declare let $: any;

@Component({
    selector: 'page-notfound',
    templateUrl: 'app/view/errors/page-notfound.component.html',
    styleUrls: ['app/view/errors/error-page.css']
})
export class PageNotfoundComponent implements OnInit {
    constructor(
        private headerService: HeaderService
    ) {}
    
    ngOnInit() {
        this.headerService.HeaderVisible = false;

        $('body').addClass('cyan');
    }
}