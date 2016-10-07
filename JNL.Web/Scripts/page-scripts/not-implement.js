(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        departs: [],

        columns: ['Id', 'Id', 'WarningTitle', 'WarningTime', 'TimeLimit', 'ImplementDeparts', 'Id'],
        builds: [
        {
            targets: [1],
            onCreateCell: function (columnValue) {
                return '监督检查预警';
            }
        },
        {
            targets: [5],
            onCreateCell: function(columnValue) {
                var ids = [];
                if (columnValue) {
                    ids = columnValue.split(',');
                }

                return page.departs.where(function(item) {
                    return ids.contains(item.Id);
                }).select(function(item) {
                    return item.Name;
                }).join();
            }
        },
        {
            targets: [6],
            onCreateCell: function (columnValue) {
                return '<td><a href="/Warn/AddWarn/{0}" title="点击进入修改">修 改</a></td>'.format(columnValue);
            }
        }],

        ajaxParams: {
            OrderField: 'UpdateTime',
            Desending: true
        },

        getConditions: function () {
            return 'HasBeganImplement=0';
        },

        init: function () {
            var _this = this;
            common.ajax({
                url: '/Common/GetList',
                data: { TableName: 'Department' }
            }).done(function(res) {
                if (res.code == 108) {
                    _this.departs = res.data;
                }
            }).done(function() {
                _this.ajaxParams.TableName = 'Warning';
                _this.ajaxParams.PageIndex = 1;
                _this.ajaxParams.PageSize = 20;
                _this.commonTable = $.commonTable('.depart-verify', {
                    columns: _this.columns,
                    builds: _this.builds,
                    ajaxParams: _this.ajaxParams,
                    getConditions: _this.getConditions
                });
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