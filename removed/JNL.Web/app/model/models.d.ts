declare module Models.ViewModel {
	interface JSONReturnVM<T> {
		data: T;
		errormessage: string;
		haserror: boolean;
	}
}

declare module Models.Params {
    interface GetListParams {
        tableName: string,
        pageIndex: number,
        pageSize: number,
        orderField: string,
        desending: boolean,
        conditions: Array<string>
    }
}