(function(window, $) {
    $(function() {
        page.init();
        page.bindEvents();
    });

    window.page = {
        table: null,

        init: function () {
            // 初始化部门选择下拉列表
            $.initSelect({
                selectors: ['#searchDepart'],
                ajaxUrl: '/Common/GetList',
                getAjaxParams: function() {
                    return { TableName: 'Department' };
                },
                textField: 'Name',
                valueField: 'Id',
                afterBuilt: function($select) {
                    $select.material_select();
                }
            });

            // 初始化字典型下拉列表
            common.loadDictionaries([
                { type: 1, selector: '#searchFlag', value: 'Id' },
                { type: 3, selector: '#searchType', value: 'Id' },
                { type: 6, selector: '#searchPosition', value: 'Id' },
                { type: 4, selector: '#searchPolitical', value: 'Id' }
            ]);

            // 时间选择器
            common.pickdate();

            // 初始化表格
            this.table = $.commonTable('.data-table', {
                columns: ['Name', 'Gender', 'SalaryId', 'WorkId', 'Department', 'WorkType', 'Position', 'HireDate', 'Id'],
                builds: [
                    {
                        targets: [7],
                        onCreateCell: function(value, data) {
                            var hireDate = data.HireDate || '';
                            if (hireDate) {
                                return common.processDate(hireDate).format('yyyy-MM-dd');
                            }

                            return hireDate;
                        }
                    },
                    {
                        targets: [8],
                        onCreateCell: function(value) {
                            return '<td><a href="/Admin/EditStaff/{0}" title="点击进入修改页面"><i class="small mdi-editor-border-color"></i></a></td>'.format(value);
                        }
                    }
                ],
                ajaxUrl: '/Common/GetList',
                ajaxParams: {
                    TableName: 'ViewStaff',
                    PageIndex: 1,
                    PageSize: 20,
                    OrderField: 'UpdateTime',
                    Desending: true
                },
                getConditions: function() {
                    var conditions = [];

                    $('.condition').each(function() {
                        var $condition = $(this),
                            value = $condition.val(),
                            condition = $condition.data('condition');

                        if (value != '' && value != -1 && value != undefined) {
                            condition = condition.replace('{{value}}', value);
                            conditions.push(condition);
                        }
                    });

                    return conditions;
                }
            });
        },

        bindEvents: function () {

            var _this = this;

            $('#btnSearch').on('click', function() {
                _this.table.setPageIndex(1);
                _this.table.loadData();
            });
        }
    };
})(window, jQuery);