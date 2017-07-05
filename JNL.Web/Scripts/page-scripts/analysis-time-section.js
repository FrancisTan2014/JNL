(function (window, $) {
    $(function () {
        common.pickdate();

        $.initSelect({
            selectors: ['#DepartmentId'],
            ajaxUrl: '/Common/GetList',
            getAjaxParams: function () {
                return { TableName: 'Department' };
            },
            textField: 'Name',
            valueField: 'Id',
            selectedValue: -1,
            afterBuilt: function ($select) {
                $select.material_select();
            }
        });

        $('#btnSearch').on('click', function () {
            var params = getAjaxParams();
            if (params) {
                common.ajax({
                    url: '?',
                    data: params
                }).done(function (res) {
                    if ($.isArray(res)) {
                        if (params.type == 1) {
                            buildTable(res);
                        } else {
                            buildChart(res);
                        }
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

        var departId = $('#DepartmentId').val();

        return {
            type: type, start: start, end: end, depart: departId
        };
    }

    function buildTable(data) {
        clearTable();
        showTable();

        var $tbody = $('.data-table>tbody');

        var total = { Name: '合计', Hx: 0, Jy: 0, Je: 0, Yi: 0, Bi: 0, Xx: 0, Hj: 0 };
        data.forEach(function (item) {
            var $tr = buildTr(item, total);
            $tbody.append($tr);
        });

        var $totalTr = buildTr(total, total);
        $tbody.append($totalTr);
    }

    function buildTr(data, counter) {
        var $tr = $('<tr></tr>');
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                $tr.append('<td>{0}</td>'.format(data[key]));

                if (key != 'Name') {
                    counter[key] += data[key];
                }
            }
        }

        return $tr;
    }

    function buildEchartSeries(data, key, name) {
        return {
            name: name,
            type: 'bar',
            stack: '总量',
            label: {
                normal: {
                    show: true,
                    position: 'insideRight'
                }
            },
            data: data.select(function (item) { return item[key] })
        };
    }

    function buildChart(data) {
        showChart();

        var departs = data.select(function (item) { return item.Name });
        var series = [
            buildEchartSeries(data, 'Hx', '红线'),
            buildEchartSeries(data, 'Jy', '甲Ⅰ'),
            buildEchartSeries(data, 'Je', '甲Ⅱ'),
            buildEchartSeries(data, 'Yi', '乙'),
            buildEchartSeries(data, 'Bi', '丙'),
            buildEchartSeries(data, 'Xx', '信息'),
        ];

        var chartBox = $('.chart-box')[0];
        var chart = echarts.init(chartBox);

        chart.setOption({
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            legend: {
                data: ['红线', '甲Ⅰ', '甲Ⅱ', '乙', '丙', '信息']
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: {
                type: 'value'
            },
            yAxis: {
                type: 'category',
                data: departs
            },
            series: series
        });
    }

    function showChart() {
        $('.chart-box').show().prev().hide();
    }

    function showTable() {
        $('.data-table').show().next().hide();
    }

    function clearTable() {
        $('.data-table>tbody').empty();
    }
})(window, jQuery);