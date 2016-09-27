import { Component, AfterViewInit } from '@angular/core';

declare var $: any;

@Component({
    selector: 'my-header',
    templateUrl: 'app/view/layout/header.component.html',
    styleUrls: ['app/view/layout/header.component.css'],
})
export class HeaderComponent implements AfterViewInit {
    constructor() { }

    ngAfterViewInit() {
        let initDropdown = function () {
            $('.dropdown-button').dropdown({
                inDuration: 300,
                outDuration: 225,
                constrain_width: false, // Does not change width of dropdown to that of the activator
                hover: true, // Activate on hover
                gutter: 0, // Spacing from edge
                belowOrigin: true // Displays dropdown below the button
            });
        };
        initDropdown();

        $('.second-menu').on('mouseenter', function () {
            let $target = $($(this).data('target'));
            let top = this.offsetParent.offsetTop;
            let left = this.offsetParent.offsetLeft + this.offsetParent.offsetParent.offsetLeft + this.clientWidth;
            $target.css({ top: top, left: left, display: 'block' }).animate({ 'opacity': 1 }, 223);

            $(this).addClass('active');
        }).on('mouseleave', function (e) {
            let $target = $($(this).data('target'));
            let minY = parseInt($target.css('top'));
            let minX = parseInt($target.css('left'));
            let maxY = minY + $target[0].clientHeight;
            let maxX = minX + $target[0].clientWidth;

            if (e.pageX >= minX && e.pageX <= maxX && e.pageY >= minY && e.pageY <= maxY) {
                return false;
            } else {
                $target.hide();
                $(this).removeClass('active');
            }
        });

        $('.third-menu').on('mouseleave', function () {
            $(this).hide();
            $('.second-menu').parent().css({ 'display': 'none' });

            initDropdown();
        });
    }
}