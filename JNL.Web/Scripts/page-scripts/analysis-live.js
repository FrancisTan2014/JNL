(function (window, $) {
    $(function () {
        var year = new Date().getFullYear(),
            $year = $('#searchYear'),
            $month = $('#searchMonth');

        for (var i = 0; i < 10; i++) {
            $year.append('<option value="{0}">{1}</option>'.format(year - i, year - i));
        }
        $year.material_select();

        for (var j = 0; j < 12; j++) {
            var month = j + 1,
                text = month < 10 ? '0' + month : month;
            $month.append('<option value="{0}">{1}</option>'.format(month, text));
        }
        $month.material_select();

        $('#btnSearch').on('click', function() {
            var month = $('#searchMonth').val(),
                year = $('#searchYear').val();
            loadData(month, year);
        });

        $('#btnSearch').click();
    });
    
    function loadData(month, year) {
        common.ajax({
            url: '?',
            data: {
                year: year,
                month: month
            }
        }).done(function (res) {
            var $dList = $('#dList').empty(),
                $nList = $('#nList').empty();

            function buildTr(item) {
                return '<tr><td>{Model}</td><td>{Gydq}</td><td>{Dydqjdxl}</td><td>{Dj}</td><td>{Dzxl}</td><td>{Zdxt}</td><td>{Zxb}</td><td>{Aqzb}</td><td>{Ctjqt}</td><td>{Cyj}</td><td>{Fzcd}</td></tr>'.format(item);
            }

            var dList = res.where(function (item) { return item.Type === '电力机车'; });
            dList.forEach(function (item) {
                $dList.append(buildTr(item));
            });

            var nList = res.where(function (item) { return item.Type === '内燃机车'; });
            nList.forEach(function (item) {
                $nList.append(buildTr(item));
            });
        });
    }

})(window, jQuery);