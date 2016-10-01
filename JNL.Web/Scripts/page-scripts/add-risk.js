(function(window, $) {
    $(function() {
        page.init();
        page.bindEvents();
    });

    window.page = {
        init: function() {
            $('#dtBox').DateTimePicker();
        },

        itelligenceConfig: {
            ajaxUrl: '/Common/GetList',
            getAjaxParams: function(input) {
                return {
                    TableName: 'ViewStaff',
                    Fields: 'Id###WorkId###SalaryId###Name###Department',
                    PageIndex: 1,
                    PageSize: 50,
                    Conditions: '(SalaryId LIKE \'{0}%\' OR WorkId LIKE \'{0}%\' OR Name LIKE \'{0}%\')'.format(input)
                };
            },
            buildDropdownItem: function(data) {
                return '{0} | {1} | {2}'.format(data.SalaryId, data.Name, data.WorkId);
            },
            afterSelected: function(data, $input) {
                $input.prev().val(data.Id)
                    .parent().next().find('input').val(data.Department || '未知部门');
            }
        },

        bindEvents: function() {
            var _this = this;
            $.inputIntelligence('#RespondStaffId', _this.itelligenceConfig);
            $.inputIntelligence('#ReportStaffId', _this.itelligenceConfig);
        }
    };
})(window, jQuery);