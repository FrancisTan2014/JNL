"use strict";
var User = (function () {
    function User(workId, password, name, departmentId, departmentName) {
        this.workId = workId;
        this.password = password;
        this.name = name;
        this.departmentId = departmentId;
        this.departmentName = departmentName;
    }
    return User;
}());
exports.User = User;
//# sourceMappingURL=User.js.map