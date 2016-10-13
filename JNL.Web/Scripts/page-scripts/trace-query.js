(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        departs: [],

        columns: ['AddTime', 'TraceType', 'ResponseDepartmentIds', 'TraceContent', 'FilePath'],
        builds: [
        {
            targets: [1],
            onCreateCell: function (columnValue) {
                return columnValue == 1 ? '局追' : '段追';
            }
        },
        {
            targets: [2],
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
            targets: [3],
            onCreateCell: function (value) {
                if (value && value.length > 60) {
                    return value.substr(0, 60) + '...';
                }

                return value;
            }
        },
        {
            targets: [4],
            onCreateCell: function (columnValue, model) {
                if (!columnValue) {
                    return '无附件';
                }

                return '<td><a href="{0}" download="{1}"><i class="small mdi-file-file-download"></i></a></td>'.format(columnValue, model.FileName);
            }
        }],

        ajaxParams: {
            OrderField: 'UpdateTime',
            Desending: true
        },

        getConditions: function () {
            var conditions = [];

            var depart = $('#searchDepart').val();
            if (depart > 0) {
                conditions.push('(ResponseDepartmentIds LIKE \'%,{0},%\' OR ResponseDepartmentIds LIKE \'{0},%\' OR ResponseDepartmentIds LIKE \'%,{0}\')'.format(depart));
            }

            var start = $('#searchStart').val();
            if (start) {
                conditions.push('AddTime>\'{0}\''.format(start));
            }

            var end = $('#searchEnd').val();
            if (end) {
                end += ' 23:59:59';
                conditions.push('AddTime<\'{0}\''.format(end));
            }

            return conditions;
        },

        init: function () {
            var _this = this;

            common.pickdate();

            $.initSelect({
                selectors: ['#searchDepart'],
                ajaxUrl: '/Common/GetList',
                textField: 'Name',
                valueField: 'Id',
                getAjaxParams: function() {
                    return {
                        TableName: 'Department'
                    };
                },
                afterBuilt: function($select) {
                    $select.material_select();
                }
            });

            common.ajax({
                url: '/Common/GetList',
                data: { TableName: 'Department' }
            }).done(function (res) {
                if (res.code == 108) {
                    _this.departs = res.data;
                }
            }).done(function () {
                _this.ajaxParams.TableName = 'TraceInfo';
                _this.ajaxParams.PageIndex = 1;
                _this.ajaxParams.PageSize = 20;
                _this.commonTable = $.commonTable('.data-table', {
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