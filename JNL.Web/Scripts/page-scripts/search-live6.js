(function(window, $) {
    $(function() {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,

        init: function () {
            var _this = this;

            $.initSelect({
                selectors: '#searchType',
                ajaxUrl: '/Common/GetList',
                textField: 'Name',
                valueField: 'Id',
                getAjaxParams: function () {
                    return {
                        TableName: 'Dictionaries',
                        Conditions: 'Type=12'
                    };
                },
                afterBuilt: function ($select) {
                    $select.material_select();
                }
            });

            $('.datepicker').pickadate({
                selectMonths: true, // Creates a dropdown to control month
                selectYears: 15, // Creates a dropdown of 15 years to control year
                format: 'yyyy-mm-dd'
            });

            _this.ajaxParams.TableName = 'ViewLocoQuality6';
            _this.ajaxParams.PageIndex = 1;
            _this.ajaxParams.PageSize = 20;
            _this.commonTable = $.commonTable('.depart-verify', {
                columns: _this.columns,
                builds: _this.builds,
                ajaxParams: _this.ajaxParams,
                getConditions: _this.getConditions
            });
        },

        columns: ['LocoModel', 'LocoNumber', 'StartRepair', 'RepairMethod', 'RepairStaff', 'LivingItem', 'BrokenPlace', 'RepairDesc'],
        builds: [
        {
            targets: [2],
            onCreateCell: function (columnValue, data) {
                try {
                    var end = common.processDate(data.EndRepair).format('yyyy-MM-dd HH:mm:ss');
                    return '{0} ~ {1}'.format(columnValue, end);
                } catch (e) {
                    return columnValue;
                } 
            }
        }],

        ajaxParams: {
            OrderField: 'ReportTime',
            Desending: true
        },

        getConditions: function () {
            var conditions = [];

            var type = $('#searchType').val();
            if (type > 0) {
                conditions.push('LocoTypeId='+type);
            }

            var model = $('#searchModel').val();
            if (model > 0) {
                conditions.push('LocoModelId=' + model);
            }

            var number = $('#searchNumber').val();
            if (number > 0) {
                conditions.push('LocomotiveId=' + number);
            }

            var start = $('#searchStar').val();
            if (start) {
                conditions.push('ReportTime>\'{0}\''.format(start));
            }

            var end = $('#searchEnd').val();
            if (end) {
                conditions.push('ReportTime<\'{0}\''.format(end + ' 23:59:59'));
            }

            return conditions;
        },

        bindEvents: function () {
            var _this = this;

            $('#searchType').on('change', function () {
                var parentId = $(this).val();
                $.initSelect({
                    selectors: '#searchModel',
                    ajaxUrl: '/Common/GetList',
                    textField: 'Name',
                    valueField: 'Id',
                    getAjaxParams: function () {
                        return {
                            TableName: 'Dictionaries',
                            Conditions: 'Type=13 AND ParentId=' + parentId
                        };
                    },
                    beforeBuilt: function ($select) {
                        $select.find('option:not(:first)').remove();
                    },
                    afterBuilt: function ($select) {
                        $select.material_select();
                    }
                });
            });

            $('#searchModel').on('change', function () {
                var parentId = $(this).val();
                $.initSelect({
                    selectors: '#searchNumber',
                    ajaxUrl: '/Common/GetList',
                    textField: 'Name',
                    valueField: 'Id',
                    getAjaxParams: function () {
                        return {
                            TableName: 'Dictionaries',
                            Conditions: 'Type=14 AND ParentId=' + parentId
                        };
                    },
                    beforeBuilt: function ($select) {
                        $select.find('option:not(:first)').remove();
                    },
                    afterBuilt: function ($select) {
                        $select.material_select();
                    }
                });
            });

            $('#btnSearch').on('click', function () {
                _this.commonTable.setPageIndex(1);
                _this.commonTable.loadData();
            });
        }
    };

})(window, jQuery);