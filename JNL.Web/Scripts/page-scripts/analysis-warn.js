(function(window, $) {
    $(function() {
        page.init();
        page.bindEvents();

        $('#btnSearch').click();
    });

    window.page = {
        init: function() {
            common.pickdate();
        },

        bindEvents: function() {
            $('#btnSearch').on('click', function() {
                var start = $('#searchStart').val() || '';
                var end = $('#searchEnd').val() || '';

                common.ajax({
                    url: '?',
                    data: {
                        startTime: start,
                        endTime: end
                    }
                }).done(function(data) {
                    var $head = $('.data-table>thead>tr').find('th:not(:first)').remove().end(),
                        $warnRow = $('.data-table>tbody>tr:first').find('td:not(:first)').remove().end(),
                        $riskRow = $('.data-table>tbody>tr:last').find('td:not(:first)').remove().end();

                    data.forEach(function(item) {
                        $head.append('<th>{0}</th>'.format(item.Name));
                        $warnRow.append('<td>{0}</td>'.format(item.Warn));
                        $riskRow.append('<td>{0}</td>'.format(item.Risk));
                    });

                    var warnTotal = data.sum(function (item) { return item.Warn; });
                    var riskTotal = data.sum(function (item) { return item.Risk; });
                    $head.append('<th>全段</th>');
                    $warnRow.append('<td>{0}</td>'.format(warnTotal));
                    $riskRow.append('<td>{0}</td>'.format(riskTotal));
                });
            });
        }
    };
})(window, jQuery);