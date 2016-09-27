"use strict";
var router_1 = require('@angular/router');
var login_component_1 = require('./view/login/login.component');
var basic_files_component_1 = require('./view/basic-files/basic-files.component');
var page_notfound_component_1 = require('./view/errors/page-notfound.component');
var appRoutes = [
    {
        path: '',
        component: login_component_1.LoginComponent
    },
    {
        path: 'login',
        component: login_component_1.LoginComponent
    },
    {
        path: 'basic/:fileType/:level',
        component: basic_files_component_1.BasicFilesComponent
    },
    {
        path: 'notfound',
        component: page_notfound_component_1.PageNotfoundComponent
    }
];
exports.router = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.router.js.map