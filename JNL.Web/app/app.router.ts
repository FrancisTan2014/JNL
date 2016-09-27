import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './view/login/login.component';
import { BasicFilesComponent } from './view/basic-files/basic-files.component';
import { PageNotfoundComponent } from './view/errors/page-notfound.component';

const appRoutes: Routes = [
    {
        path: '',
        component: LoginComponent
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'basic/:fileType/:level',
        component: BasicFilesComponent
    },
    {
        path: 'notfound',
        component: PageNotfoundComponent
    }
];

export const router = RouterModule.forRoot(appRoutes);