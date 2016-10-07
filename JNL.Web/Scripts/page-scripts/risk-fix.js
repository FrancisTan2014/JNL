(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        columns: ['AddTime', 'ReportStaffDepart', 'RiskType', 'RiskRespondDepart', 'HasDealed', 'RiskSummary', 'RiskDetails', 'RiskId'],
        builds: [
        {
            targets: [4],
            onCreateCell: function (columnValue) {
                var text = columnValue ? '已处置' : '未处置';
                return '<td>{0}</td>'.format(text);
            }
        },
        {
            targets: [5,6],
            onCreateCell: function (columnValue) {
                var text = columnValue;
                if (text.length > 30) {
                    text = text.substr(0, 30) + '......';
                }

                return '<td>{0}</td>'.format(text);
            }
        },
        {
            targets: [7],
            onCreateCell: function (columnValue) {
                return '<td><a href="/Risk/Write/{0}">处置</a></td>'.format(columnValue);
            }
        }],

        ajaxParams: {
            OrderField: 'AddTime',
            Desending: true
        },

        getConditions: function () {
            var status = $('.fix-form').data('status');
            var depart = $('.fix-form').data('depart');
            if (status) {
                return 'NeedWriteFixDesc=1 AND VerifyStatus={0} AND RiskRespondDepartId={1}'.format(status, depart);
            }
        },

        init: function () {

            this.ajaxParams.TableName = 'ViewRiskResponse';
            this.ajaxParams.PageIndex = 1;
            this.ajaxParams.PageSize = 20;

            var _this = this;
            _this.commonTable = $.commonTable('.fix-form', {
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