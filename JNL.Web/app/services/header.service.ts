import { Injectable } from '@angular/core';

@Injectable()
export class HeaderService {
    // 控制header显示与隐藏的服务
    private headerVisible: boolean = true;

    set HeaderVisible(value: boolean) {
        this.headerVisible = value;
    }

    get HeaderVisible() {
        return this.headerVisible;
    }
}