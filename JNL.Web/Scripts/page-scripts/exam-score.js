(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,

        init: function () {
            var _this = this;

            $.inputIntelligence('#searchName', {
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
                    $input.prev().val(data.Id);
                }
            });

            common.loadDictionaries([
                { type: 6, selector: '#searchPost', value: 'Id' }
            ]);

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
                columns: ['Id', 'StaffName', 'WorkNo', 'Position', 'WorkPlace', 'ExamTheme', 'ExamSubject', 'Score', 'ExamTime'],
                builds: [
                {
                    targets: [8],
                    onCreateCell: function (c, data) {
                        try {
                            return common.processDate(data.ExamTime).format('yyyy-MM-dd');
                        } catch (e) {
                            console.info(e);
                            return c;
                        }
                    }
                }],
                ajaxParams: {
                    TableName: 'ViewExamScore',
                    OrderField: 'AddTime',
                    Desending: true,
                    PageIndex: 1,
                    PageSize: 20
                },
                getConditions: function () {
                    var conditions = [];

                    var staffId = $('#searchStaff').val();
                    if (staffId > 0) {
                        conditions.push('StaffId=' + staffId);
                    }

                    var departId = $('#searchDepart').val();
                    if (departId > 0) {
                        conditions.push('DepartmentId=' + departId);
                    }

                    var postId = $('#searchPost').val();
                    if (postId > 0) {
                        conditions.push('PositionId=' + postId);
                    }

                    return conditions;
                }
            });
        },

        bindEvents: function () {
            var _this = this;

            $('#searchName').on('keydown', function() {
                $(this).prev().val(-1);
            });

            $('#btnSearch').on('click', function () {
                _this.commonTable.setPageIndex(1);
                _this.commonTable.loadData();
            });
        }
    };
})(window, jQuery);