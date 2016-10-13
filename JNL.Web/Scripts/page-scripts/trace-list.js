(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        departs: [],

        columns: ['AddTime', 'TraceType', 'ResponseDepartmentIds', 'TraceContent', 'Id'],
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
            onCreateCell: function(value) {
                if (value && value.length > 60) {
                    return value.substr(0, 60) + '...';
                }

                return value;
            }
        },
        {
            targets: [4],
            onCreateCell: function (columnValue) {
                return '<td><a href="/Trace/Edit/{0}" title="点击进入修改" class="btn btn-floating"><i class="mdi-editor-border-color"></i></a>&nbsp;&nbsp;<a href="javascript:page.deleteTrace({0});" title="点击删除信息" class="btn btn-floating delete-trace" data-id={0}><i class="small mdi-action-delete"></i></a></td>'.format(columnValue);
            }
        }],

        ajaxParams: {
            OrderField: 'UpdateTime',
            Desending: true
        },

        getConditions: function () {
            var conditions = [];

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

        deleteTrace: function (id) {
            var _this = this;
            if (confirm('您确定删除此追踪信息吗？')) {
                common.ajax({
                    url: '/Common/DeleteData',
                    data: {
                        target: 'TraceInfo',
                        id: id
                    }
                }).done(function(res) {
                    if (res.code == 100) {
                        Materialize.toast('删除成功:(=', 1000, '', function() {
                            _this.removeRow(id);
                        });
                    } else {
                        Materialize.toast('删除失败，请稍后重试:(=', 3000);
                    }
                });
            }
        },

        removeRow: function(id) {
            $('.delete-trace').each(function() {
                var $this = $(this),
                    traceId = $this.data('id');
                if (traceId == id) {
                    $this.parents('tr').remove();
                    return false;
                }
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