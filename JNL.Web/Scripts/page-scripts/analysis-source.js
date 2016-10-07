(function (window, $) {
    function search() {
        var type = $('#searchType').val();
        var start = $('#searchStart').val();
        if (!start) {
            Materialize.toast('请选择开始时间', 3000);
        }

        var end = $('#searchEnd').val();
        if (!end) {
            Materialize.toast('请选择结束时间', 3000);
        } else {
            end += " 23:59:59";
        }

        var $firstTh = $('th:first');
        if (type == 1) {
            $firstTh.text('提报平台');
        } else {
            $firstTh.text('提报部门');
        }

        var $tBody = $('tbody');
        $tBody.empty();

        common.ajax({
            url: '?',
            data: {
                type: type,
                start: start,
                end: end
            }
        }).done(function(res) {
            if (res.code == 108) {
                res.data.forEach(function(item) {
                    $tBody.append('<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>'.format(item.Name, item.Red, item.One, item.Two, item.Three, item.Four, item.Info, item.Total));
                });
            }
        });
    }

    $(function () {
        $('#dtBox').DateTimePicker();

        search();

        $('#btnSearchResult').on('click', function() {
            search();
        });
    });
})(window, jQuery);