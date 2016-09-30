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
var HeaderComponent = (function () {
    function HeaderComponent() {
    }
    HeaderComponent.prototype.ngAfterViewInit = function () {
        var initDropdown = function () {
            $('.dropdown-button').dropdown({
                inDuration: 300,
                outDuration: 225,
                constrain_width: false,
                hover: true,
                gutter: 0,
                belowOrigin: true // Displays dropdown below the button
            });
        };
        initDropdown();
        $('.second-menu').on('mouseenter', function () {
            var $target = $($(this).data('target'));
            var top = this.offsetParent.offsetTop;
            var left = this.offsetParent.offsetLeft + this.offsetParent.offsetParent.offsetLeft + this.clientWidth;
            $target.css({ top: top, left: left, display: 'block' }).animate({ 'opacity': 1 }, 223);
            $(this).addClass('active');
        }).on('mouseleave', function (e) {
            var $target = $($(this).data('target'));
            var minY = parseInt($target.css('top'));
            var minX = parseInt($target.css('left'));
            var maxY = minY + $target[0].clientHeight;
            var maxX = minX + $target[0].clientWidth;
            if (e.pageX >= minX && e.pageX <= maxX && e.pageY >= minY && e.pageY <= maxY) {
                return false;
            }
            else {
                $target.hide();
                $(this).removeClass('active');
            }
        });
        $('.third-menu').on('mouseleave', function () {
            $(this).hide();
            $('.second-menu').parent().css({ 'display': 'none' });
            initDropdown();
        });
    };
    HeaderComponent = __decorate([
        core_1.Component({
            selector: 'my-header',
            templateUrl: 'app/view/layout/header.component.html',
            styleUrls: ['app/view/layout/header.component.css'],
        }), 
        __metadata('design:paramtypes', [])
    ], HeaderComponent);
    return HeaderComponent;
}());
exports.HeaderComponent = HeaderComponent;
//# sourceMappingURL=header.component.js.map