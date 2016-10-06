(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        columns: ['Id', 'ReportStaffDepart', 'AddTime', 'RiskTopestName', 'RiskSecondLevelName', 'RiskSummary', 'VerifyStatus', 'VerifyStatus', 'Id'],
        builds: [
        {
            targets: [6],
            onCreateCell: function (columnValue) {
                var text = columnValue == 1 ? '未审核' : (
                        columnValue == 3 ? '需重新审核' : '已审核'
                    );
                return '<td>{0}</td>'.format(text);
            }
        },
        {
            targets: [7],
            onCreateCell: function (columnValue) {
                var text = columnValue == 2 ? '未审核' : (
                        columnValue == 3 ? '已否决' : '待部门审核'
                    );
                return '<td>{0}</td>'.format(text);
            }
        },
        {
            targets: [8],
            onCreateCell: function (columnValue) {
                return '<td><a href="/Risk/UpdateRisk/{0}" title="点击查看详情">进入审核</a></td>'.format(columnValue);
            }
        }],

        ajaxParams: {
            OrderField: 'AddTime',
            Desending: true
        },

        getConditions: function () {
            var depart = $('.depart-verify').data('depart');
            if (depart) {
                return '(VerifyStatus=1 OR VerifyStatus=3) AND ReportStaffDepartId=' + depart;
            } else {
                return 'VerifyStatus=2';
            }
        },
        
        init: function () {

            this.ajaxParams.TableName = 'ViewRiskInfo';
            this.ajaxParams.PageIndex = 1;
            this.ajaxParams.PageSize = 20;

            var _this = this;
            _this.commonTable = $.commonTable('.depart-verify', {
                columns: _this.columns,
                builds: _this.builds,
                ajaxParams: _this.ajaxParams,
                getConditions: _this.getConditions
            });
        },

        bindEvents: function () {
            var _this = this;
            $('#btnSearch').on('click', function () {
                _this.commonTable.setPageIndex(1);
                _this.commonTable.loadData();
            });
        }
    };
})(window, jQuery);