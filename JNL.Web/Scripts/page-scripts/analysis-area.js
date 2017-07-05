(function(window, $) {
    $(function() {
        common.pickdate();

        $('#btnSearch').on('click', function () {
            clearTable();

            var params = getAjaxParams();
            if (params) {
                common.ajax({
                    url: '?',
                    data: params
                }).done(function(res) {
                    if ($.isArray(res)) {
                        initTitle(params.type);

                        var $tbody = $('.data-table>tbody');
                        res.forEach(function(item) {
                            var $tr = $('<tr></tr>');
                            buildTr(params.type, item, $tr);

                            $tbody.append($tr);
                        });
                    } else {
                        Materialize.toast('没有查询到结果！', 3000);
                    }
                });
            }
        });

        $('#btnSearch').click();
    });

    // 获取异步请求参数
    function getAjaxParams() {
        var type = $('#searchType').val();
        var start = $('#searchStart').val();
        if (!start) {
            Materialize.toast('请选择开始日期', 3000);
            return null;
        }
        var end = $('#searchEnd').val();
        if (!end) {
            Materialize.toast('请选择结束日期', 3000);
            return null;
        }
        end += ' 23:59:59';

        return {
            type: type, start: start, end: end
        };
    }

    function buildTr(type, data, $tr) {
        if (type == 1) {
            $tr.append('<td>{0}</td><td>{1}</td><td>{2}</td>'.format(data.line, data.station, data.count));
        } else {
            $tr.append('<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>'.format(data.line, data.first, data.last, data.count));
        }
    }

    function initTitle(type) {
        var $title = $('.data-table>thead>tr');
        if (type == 1) {
            $title.append('<th>发生线路</th><th>站场</th><th>数量</th>');
        } else {
            $title.append('<th>发生线路</th><th>站1</th> <th>站2</th> <th>数量</th>');
        }
    }

    function clearTable() {
        $('.data-table').find('thead>tr').empty().end()
            .find('tbody').empty();
    }
})(window, jQuery);