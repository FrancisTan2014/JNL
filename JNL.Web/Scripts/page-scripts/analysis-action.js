(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();

        $('#btnSearch').click();
    });

    window.page = {
        init: function () {
            
        },

        bindEvents: function () {
            $('#btnSearch').on('click', function () {
                var year = $('#searchYear').val();

                common.ajax({
                    url: '?',
                    data: {
                        year: year
                    }
                }).done(function (data) {
                    if (data instanceof Array && data.length > 0) {
                        var $head = $('.data-table>thead>tr').find('th:not(:first)').remove().end(),
                            $trs = $('.data-table>tbody>tr'),
                            $yellow = $trs.eq(0).find('td:not(:first)').remove().end(),
                            $orange = $trs.eq(1).find('td:not(:first)').remove().end(),
                            $red = $trs.eq(2).find('td:not(:first)').remove().end(),
                            yellowCount = 0,
                            orangeCount = 0,
                            redCount = 0;


                        data.forEach(function(item) {
                            $head.append('<th>{0}</th>'.format(item.name));
                            $yellow.append('<td>{0}</td>'.format(item.yellow));
                            $orange.append('<td>{0}</td>'.format(item.orange));
                            $red.append('<td>{0}</td>'.format(item.red));

                            yellowCount += +item.yellow;
                            orangeCount += +item.orange;
                            redCount += +item.redCount;
                        });

                        $head.append('<th>全段</th>');
                        $yellow.append('<td>{0}</td>'.format(yellowCount));
                        $orange.append('<td>{0}</td>'.format(orangeCount));
                        $red.append('<td>{0}</td>'.format(redCount));
                    } else {
                        Materialize.toast('没有查询到结果！', 3000);
                    }
                });
            });
        }
    };
})(window, jQuery);