import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { HeaderService } from './../../services/header.service';

import { MockData } from './../../mock/mock';

import 'rxjs/Rx';

@Component({
    selector: 'basic-files',
    templateUrl: 'app/view/basic-files/basic-files.component.html'
})
export class BasicFilesComponent implements OnInit {
    fileTypes = ['技术规章', '企业标准', '制度措施'];
    levels = ['总公司', '铁路局', '机务段'];
    files = MockData.files;
    title: string;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private headerService: HeaderService
    ) { }

    ngOnInit() {
        this.route.params.forEach((params: Params) => {
            let fileType = +params['fileType'];
            let level = +params['level'];

            if (fileType < 1 || fileType > 3 || level < 1 || level > 3) {
                this.router.navigate(['/notfound']);
            } else {
                this.title = `${this.fileTypes[fileType - 1]} - ${this.levels[level - 1]}`;
            }
        });
    }
}