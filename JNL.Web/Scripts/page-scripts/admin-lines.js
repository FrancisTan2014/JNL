(function (window, $) {

    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        table: null,

        // 初始化
        init: function () {

            var _this = this;

            // 初始化表格
            _this.table = $.commonTable('.data-table', {
                columns: ['Id', 'Name', 'FirstStation', 'LastStation', 'UpdateTime', 'Id'],
                builds: [
                    {
                        targets: [5],
                        onCreateCell: function (columnValue) {
                            return '<td><a href="/Admin/EditLine/{0}" title="点击修改"><i class="small mdi-editor-border-color"></i></a></td>'.format(columnValue);
                        }
                    }],
                ajaxParams: {
                    TableName: 'Line',
                    OrderField: 'UpdateTime',
                    Desending: true,
                    PageSize: 20,
                    PageIndex: 1
                },
                getConditions: function () {
                    var conditions = [];
                    $('#searchBox').find('select.initialized,input').each(function () {
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
        },

        // 绑定事件
        bindEvents: function () {

            var _this = this;

            // 点击查询
            $('#btnSearch').on('click', function () {
                _this.table.setPageIndex(1);
                _this.table.loadData();
            });
        }
    };

})(window, jQuery);