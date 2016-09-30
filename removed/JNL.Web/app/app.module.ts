import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { MdMenuModule } from '@angular2-material/menu';
import { MdIconModule } from '@angular2-material/icon';
import { Ng2BootstrapModule } from 'ng2-bootstrap/ng2-bootstrap';

import { HeaderService } from './services/header.service';
import { LoginService } from './services/login.service';

import { router } from './app.router';

import { BasicFilesComponent } from './view/basic-files/basic-files.component';
import { LoginComponent } from './view/login/login.component';
import { HeaderComponent } from './view/layout/header.component';
import { PageNotfoundComponent } from './view/errors/page-notfound.component';
// import { SpinnerComponent } from './view/spinner/spinner.component';
import { AppComponent } from './app.component';

@NgModule({
    imports: [ BrowserModule, FormsModule, Ng2BootstrapModule, 
               MdMenuModule.forRoot(), MdIconModule.forRoot(),
               router ],
    declarations: [ AppComponent, LoginComponent, HeaderComponent, BasicFilesComponent, PageNotfoundComponent ],
    providers: [ HeaderService, LoginService ],
    bootstrap: [AppComponent]
})
export class AppModule {
}