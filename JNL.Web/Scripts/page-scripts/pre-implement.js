(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        departs: [],

        columns: ['Id', 'Id', 'WarningTitle', 'WarningTime', 'TimeLimit', 'ImplementDeparts', 'ImplementDeparts', 'ImplementDeparts', 'Id'],
        builds: [
        {
            targets: [1],
            onCreateCell: function (columnValue, data) {
                var timeLimit = common.processDate(data.TimeLimit);
                return '<td class="data" data-id="{0}" data-departs="{1}" data-time="{2}">监督检查预警</td>'.format(columnValue, data.ImplementDeparts, timeLimit);
            }
        },
        {
            targets: [5],
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
            targets: [6, 7],
            onCreateCell: function() {
                return '';
            }
        },
        {
            targets: [8],
            onCreateCell: function (columnValue) {
                return '<td><a href="/Warn/AddImplement/{0}" title="点击添加响应">添加响应</a></td>'.format(columnValue);
            }
        }],

        ajaxParams: {
            OrderField: 'UpdateTime',
            Desending: true
        },

        getConditions: function () {
            return 'HasImplementedAll=0';
        },

        afterLoaded: function () {
            var _this = this;
            var $trs = $('tr:not(.dealed)');
            var warnIds = [];
            $trs.find('td.data').each(function() {
                warnIds.push($(this).data('id'));
            });

            if (warnIds.length > 0) {
                common.ajax({
                    url: '/Common/GetList',
                    data: {
                        TableName: 'ViewWarningImplement',
                        Conditions: 'WarningId IN({0})'.format(warnIds.join())
                    }
                }).done(function(res) {
                    if (res.code == 108) {
                        var list = res.data || [];

                        $trs.each(function () {
                            var $row = $(this),
                                $td = $row.find('.data'),
                                warnId = $td.data('id'),
                                departs = $td.data('departs').split(','),
                                timeLimit = $td.data('time'),
                                expired = [], // 超过期限
                                voted = []; // 被否决

                            list.where(function(item) {
                                return item.WarningId == warnId;
                            }).forEach(function(implement) {
                                departs.remove(implement.WarningId);
                                if (implement.ResponseVerifyStatus == 3) {
                                    voted.push(implement.ImplementDepart);
                                }
                            });

                            $td.find('td').eq(7).text(voted.join(' '));
                            if (new Date(timeLimit) < new Date()) {
                                expired = _this.departs.where(function(item) {
                                    return departs.contains(item.Id);
                                }).select(function(item) {
                                    return item.Name;
                                });
                            }

                            $td.find('td').eq(6).text(expired.join(' '));
                        });
                    }
                });
            }
        },

        init: function () {
            var _this = this;
            common.ajax({
                url: '/Common/GetList',
                data: { TableName: 'Department' }
            }).done(function (res) {
                if (res.code == 108) {
                    _this.departs = res.data;
                }
            }).done(function () {
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