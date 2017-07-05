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

    // 初始化表格
    var table = $.commonTable('.data-table', {
        columns: ['Id', 'Name', 'Department', 'QuotaType', 'QuotaAmmount', 'AchieveAmmount', 'Month', 'UpdateTime'],
        beforeBuildTr: function ($tr, data) {
            console.info(data);
            // 未完成显示红色，完成显示绿色
            if (data.AchieveAmmount >= data.QuotaAmmount) {
                $tr.css('color', '#00c853');
            } else {
                $tr.css('color', '#f44336');
            }
        },
        builds: [
            {
                targets: [3],
                onCreateCell: function (columnValue) {
                    return '<td>风险信息录入条数指标</td>';
                }
            },
            {
                targets: [6],
                onCreateCell: function (columnValue, data) {
                    return '<td>{0}-{1}</td>'.format(data.Year, data.Month);
                }
            }],
        ajaxParams: {
            TableName: 'ViewQuotaAchievement',
            OrderField: 'UpdateTime',
            Desending: true,
            PageSize: 20,
            PageIndex: 1
        },
        getConditions: function () {
            var conditions = [];
            $('#searchBox').find('select.initialized,input.datepicker').each(function () {
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

        // 初始化天气、列车类别下拉选择框
        common.loadDictionaries([
            { type: 9, selector: '#searchWeather', value: 'Id' },
            { type: 8, selector: '#searchLocoType', value: 'Id' }
        ]);

        // 初始化时间选择器
        common.pickdate();

        // 初始化人员智能提示
        $.inputIntelligence('#staffName', {
            ajaxUrl: '/Common/GetList',
            getAjaxParams: function (input) {
                return {
                    TableName: 'ViewStaff',
                    Fields: 'Id###WorkId###SalaryId###Name###Department',
                    PageIndex: 1,
                    PageSize: 50,
                    Conditions: '(SalaryId LIKE \'{0}%\' OR WorkId LIKE \'{0}%\' OR Name LIKE \'{0}%\')'.format(input)
                };
            },
            buildDropdownItem: function (data) {
                return '{0} | {1} | {2}'.format(data.SalaryId, data.Name, data.WorkId);
            },
            afterSelected: function (data, $input) {
                window.editStatus.staffId = data.Id;
                $input.val('{0} | {1} | {2}'.format(data.SalaryId, data.Name, data.WorkId));
            }
        });

        $('#btnSearch').on('click', function () {
            table.setPageIndex(1);
            table.loadData();
        });
    });
})(window, jQuery);