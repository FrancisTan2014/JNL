(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        commonTable: null,
        departs: [],

        columns: ['Id', 'Title', 'Creator', 'Department', 'AddTime', 'FilePath'],
        builds: [
        {
            targets: [5],
            onCreateCell: function (columnValue, data) {
                return '<td><a href="{0}" download="{1}" title="点击下载文件" class="btn waves-light waves-effect"><i class="small mdi-editor-vertical-align-bottom"></i></a></td>'.format(columnValue, data.Title);
                
            }
        }],

        ajaxParams: {
            OrderField: 'AddTime',
            Desending: true
        },

        getConditions: function () {
            var category = $('.depart-verify').data('category');
            var level = $('.depart-verify').data('level');
            return 'CategoryId={0} AND PubLevel={1}'.format(category, level);
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