(function (window, $) {
    $(function () {
        common.pickdate();

        common.loadDictionaries([
            { type: 9, selector: '#searchWeather', value: 'Id' }
        ]);

        $('#btnSearch').on('click', function () {
            clearTable();

            var params = getAjaxParams();
            if (params) {
                common.ajax({
                    url: '?',
                    data: params
                }).done(function (res) {
                    if ($.isArray(res)) {
                        var $tbody = $('.data-table>tbody');
                        res.forEach(function (item) {
                            var type = '【{0}-{1}】'.format(item.type, item.level);
                            $tbody.append('<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>'.format(type, item.weather, item.summary, item.count));
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
        var weather = $('#searchWeather').val();
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
            weatherId: weather, start: start, end: end
        };
    }

    function clearTable() {
        $('.data-table>tbody').empty();
    }
})(window, jQuery);