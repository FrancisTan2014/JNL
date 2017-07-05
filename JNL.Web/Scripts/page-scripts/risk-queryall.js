(function (window, $) {
    // 初始化部门下拉选择框
    function initDeparts(selectors) {
        $.initSelect({
            selectors: selectors,
            ajaxUrl: '/Common/GetList',
            textField: 'Name',
            valueField: 'Id',
            getAjaxParams: function () {
                return { TableName: 'Department' };
            },
            afterBuilt: function ($select) {
                $select.material_select();
            }
        });
    }

    // 初始化风险等级下拉选择框
    function initLevel(selector) {
        $.initSelect({
            selectors: [selector],
            ajaxUrl: '/Common/GetList',
            textField: 'Description',
            valueField: 'Id',
            getAjaxParams: function () {
                return {
                    TableName: 'RiskSummary',
                    Conditions: 'SecondLevelId=0 AND TopestTypeId<>0'
                };
            },
            afterBuilt: function ($select) {
                $select.material_select();
            }
        });
    }

    // 初始化来源平台下拉选择框
    function initPlatforms(selector) {
        $.initSelect({
            selectors: [selector],
            ajaxUrl: '/Common/GetList',
            textField: 'Name',
            valueField: 'Id',
            getAjaxParams: function () {
                return {
                    TableName: 'RiskType',
                    Conditions: 'ParentId<>0'
                };
            },
            afterBuilt: function ($select) {
                $select.material_select();
            }
        });
    }

    // 初始化表格
    var table = $.commonTable('.data-table', {
        columns: ['RiskType', 'ReportStaffDepart', 'OccurrenceTime', 'WeatherLike', 'Department', 'LocoServiceType', 'RiskSecondLevelName', 'RiskSummary', 'RiskDetails', 'RiskId'],
        builds: [
            {
                targets: [7,8],
                onCreateCell: function (columnValue) {
                    if (columnValue && columnValue.length > 10) {
                        return columnValue.substr(0, 10) + '...';
                    }

                    return columnValue;
                }
            },
            {
            targets: [9],
            onCreateCell: function (columnValue) {
                return '<td><a href="/Risk/Detail/{0}" title="点击查看详情"><i class="small mdi-content-link"></i></a></td>'.format(columnValue);
            }
        }],
        ajaxParams: {
            TableName: 'ViewRiskRespondRisk',
            OrderField: 'OccurrenceTime',
            Desending: true,
            PageSize: 20,
            PageIndex: 1
        },
        getConditions: function () {
            var conditions = ['VerifyStatus=4'];
            $('#searchBox').find('select.initialized,input.datepicker,input.search-text').each(function () {
                var $this = $(this),
                    value = $this.val();

                if (value != -1 && value != '' && value != undefined) {
                    var con = $this.data('condition').replace('{{value}}', value);
                    conditions.push(con);
                }
            });

            return conditions;
        }
    });

    $(function () {
        initDeparts(['#searchReport', '#searchRespond']);
        initLevel('#searchLevel');
        initPlatforms('#searchPlatform');

        // 初始化天气、列车类别下拉选择框
        common.loadDictionaries([
            { type: 9, selector: '#searchWeather', value: 'Id' },
            { type: 8, selector: '#searchLocoType', value: 'Id' }
        ]);

        // 初始化时间选择器
        common.pickdate();

        $('#btnSearch').on('click', function () {
            table.setPageIndex(1);
            table.loadData();
        });
    });
})(window, jQuery);