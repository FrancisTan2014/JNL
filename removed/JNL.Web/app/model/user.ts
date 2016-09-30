export class User {        
        constructor(
            public workId: string,
            public password: string,
            public name?: string,
            public departmentId?: number,
            public departmentName?: string
        ){}
}