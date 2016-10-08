(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        departs: [],

        columns: ['Id', 'Id', 'WarningTitle', 'WarningTime', 'ImplementDeparts', 'Id'],
        builds: [
        {
            targets: [1],
            onCreateCell: function (columnValue) {
                return '监督检查预警';
            }
        },
        {
            targets: [4],
            onCreateCell: function (columnValue) {
                var ids = [];
                if (columnValue) {
                    ids = columnValue.split(',');
                }

                return page.departs.where(function (item) {
                    return ids.contains(item.Id);
                }).select(function (item) {
                    return item.Name;
                }).join();
            }
        },
        {
            targets: [5],
            onCreateCell: function (columnValue) {
                return '<td><a href="/Warn/Verify/{0}" title="点击查看明细">查看明细</a></td>'.format(columnValue);
            }
        }],

        ajaxParams: {
            OrderField: 'UpdateTime',
            Desending: true
        },

        getConditions: function () {
            var conditions = ['HasImplementedAll=1'];

            var implementDepart = $('#searchImplement').val();
            if (implementDepart > 0) {
                conditions.push('(ImplementDeparts LIKE \'{0},%\' OR ImplementDeparts LIKE \'%,{0},%\' OR ImplementDeparts LIKE \'%,{0}\')'.format(implementDepart));
            }

            var warnDepart = $('#searchDepart').val();
            if (warnDepart > 0) {
                conditions.push('WarningDepartId='+warnDepart);
            }

            var start = $('#searchStar').val();
            if (start) {
                conditions.push('WarningTime>\'{0}\''.format(start));
            }

            var end = $('#searchEnd').val();
            if (end) {
                end += ' 23:59:59';
                conditions.push('WarningTime<\'{0}\''.format(end));
            }

            return conditions;
        },

        init: function () {
            var _this = this;
            common.ajax({
                url: '/Common/GetList',
                data: { TableName: 'Department' }
            }).done(function (res) {
                if (res.code == 108) {
                    _this.departs = res.data;

                    res.data.forEach(function(depart) {
                        $('#searchImplement').append('<option value="{0}">{1}</option>'.format(depart.Id, depart.Name));
                    });

                    $('#searchImplement').material_select();
                }
            }).done(function () {
                _this.ajaxParams.TableName = 'ViewWarning';
                _this.ajaxParams.PageIndex = 1;
                _this.ajaxParams.PageSize = 20;
                _this.commonTable = $.commonTable('.depart-verify', {
                    columns: _this.columns,
                    builds: _this.builds,
                    ajaxParams: _this.ajaxParams,
                    getConditions: _this.getConditions
                });
            });

            $('.datepicker').pickadate({
                selectMonths: true, // Creates a dropdown to control month
                selectYears: 15, // Creates a dropdown of 15 years to control year
                format: 'yyyy-mm-dd'
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