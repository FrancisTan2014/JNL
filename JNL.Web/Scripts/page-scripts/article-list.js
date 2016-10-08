(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        departs: [],

        columns: ['Id', 'Title', 'Creator', 'Department', 'AddTime', 'Id'],
        builds: [
        {
            targets: [5],
            onCreateCell: function (columnValue) {
                return '<td><a href="/Article/Scan/{0}" title="点击进入查看">查 看</a></td>'.format(columnValue);
            }
        }],

        ajaxParams: {
            OrderField: 'AddTime',
            Desending: true
        },

        getConditions: function () {
            var category = $('.depart-verify').data('category');
            return 'CategoryId=' + category;
        },

        init: function () {
            var _this = this;
            _this.ajaxParams.TableName = 'ViewArticle';
            _this.ajaxParams.PageIndex = 1;
            _this.ajaxParams.PageSize = 20;
            _this.commonTable = $.commonTable('.depart-verify', {
                columns: _this.columns,
                builds: _this.builds,
                ajaxParams: _this.ajaxParams,
                getConditions: _this.getConditions
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