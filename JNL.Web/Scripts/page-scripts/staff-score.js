(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        commonAjaxUrl: '/Common/GetList',

        init: function () {
            var _this = this;

            $.inputIntelligence('#searchName', {
                ajaxUrl: _this.commonAjaxUrl,
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
                    $input.prev().val(data.Id);
                }
            });

            $.initSelect({
                selectors: ['#searchDepart'],
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

            _this.commonTable = $.commonTable('.score-table', {
                columns: ['StaffName', 'Department', 'SalaryId', 'Position', 'MinusScore'],
                builds: [],
                ajaxParams: {
                    TableName: 'ViewStaffScore',
                    PageIndex: 1,
                    PageSize: 20
                },
                getConditions: function () {
                    var conditions = ['WorkFlagId=1'];

                    var staffId = $('#searchStaff').val();
                    if (staffId > 0) {
                        conditions.push('StaffId=' + staffId);
                    }

                    var departId = $('#searchDepart').val();
                    if (departId > 0) {
                        conditions.push('DepartmentId=' + departId);
                    }

                    var year = $('#searchYear').val();
                    if (year > 0) {
                        conditions.push('Year=' + year);
                    }

                    var month = $('#searchMonth').val();
                    if (month > 0) {
                        conditions.push('Month<=' + month);
                    }

                    return conditions;
                }
            });
        },

        bindEvents: function () {
            var _this = this;

            $('#searchName').on('keydown', function () {
                $(this).prev().val(-1);
            });

            $('#btnSearch').on('click', function () {
                var month = $('#searchMonth').val();

                var params = {};
                if (month > 0) {
                    params.TableName = 'ViewStaffScore';
                    params.OrderField = 'Id';
                    params.Desending = false;
                } else {
                    params.TableName = 'ViewStaffScoreTotal';
                    params.OrderField = 'MinusScore';
                    params.Desending = true;
                }

                _this.commonTable.changeAjaxParams(params);
                _this.commonTable.setPageIndex(1);
                _this.commonTable.loadData();
            });
        }
    };
})(window, jQuery);