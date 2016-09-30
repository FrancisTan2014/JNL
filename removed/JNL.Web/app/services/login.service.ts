import { Injectable } from '@angular/core';

import { User } from './../model/User';

@Injectable()
export class LoginService {
    private testUser: User;
    constructor(){
        this.testUser = new User('2008441101', 'prutgh990610cqdj', 'FrancisTan', 1, '集宁机务段');
    }

    private currentUser: User;

    get CurrentUser() {
        return this.currentUser;
    }

    login(workId: string, password: string) {
        console.log(`password equals: ${password === this.testUser.password}`);
        if (workId === this.testUser.workId && password === this.testUser.password) {
            this.currentUser = this.testUser;
            return true;
        }

        return false;
    }

    logout() {
        this.currentUser = null;
    }
}