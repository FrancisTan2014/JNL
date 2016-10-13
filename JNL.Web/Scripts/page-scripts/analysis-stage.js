(function (window, $) {
    // 初始化部门下拉选择框
    function initSelect() {
        $.initSelect({
            selectors: ['#searchDepart'],
            ajaxUrl: '/Common/GetList',
            textField: 'Name',
            valueField: 'Id',
            getAjaxParams: function () {
                return {
                    TableName: 'Department'
                };
            },
            afterBuilt: function ($select) {
                $select.material_select();
            }
        });
    }

    // 获取异步请求参数
    function getAjaxParams() {
        var params = {}, valid = true;
        $('.datepicker').each(function() {
            var $this = $(this),
                value = $this.val();

            if (!value) {
                var tips = $this.prop('placeholder') + '不能为空:(=';
                Materialize.toast(tips, 3000);

                valid = false;
                return false;
            } else {
                var key = $this.data('key');
                params[key] = value;
            }
        });

        var departId = $('#searchDepart').val();
        params.departId = departId;

        if (!valid) {
            return null;
        }

        return params;
    }

    // 填充表格
    function fillTable(data) {
        var $tbody = $('.data-table>tbody').empty();
        data.forEach(function(item) {
            $tbody.append('<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>'.format(item.name, item.daily1, item.daily2, item.total1, item.total2));
        });
    }

    function fillCharts(data) {
        var legends = data.select(function (item) { return item.name });

        // 日均
        var dailySeries = data.select(function (item) {
            return {
                name: item.name,
                type: 'bar',
                data: [item.daily1, item.daily2]
            };
        });

        var dailyChartDiv = $('.chart-box:first')[0];
        drawChart(dailyChartDiv, legends, dailySeries, true);

        // 总数
        var totalSeries = data.select(function (item) {
            return {
                name: item.name,
                type: 'bar',
                data: [item.total1, item.total2]
            }
        });

        var totalChartDiv = $('.chart-box:last')[0];
        drawChart(totalChartDiv, legends, totalSeries);
    }

    function drawChart(dom, legends, series, isDaily) {
        var desc = isDaily ? '日均' : '总数';

        var chart = echarts.init(dom);
        chart.setOption({
            tooltip: {
                trigger: 'axis',
                axisPointer: { type: 'shadow' }
            },
            legend: {
                orient: 'horizontal',
                right: 'left',
                data: legends
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: [
                 {
                     type: 'value'
                 }
            ],
            yAxis: [
               {
                   type: 'category',
                   data: ['一阶段' + desc, '二阶段' + desc]
               }
            ],
            series: series
        });
    }

    $(function () {
        initSelect();
        common.pickdate();

        $('#btnSearch').on('click', function() {
            var params = getAjaxParams();
            if (null !== params) {
                common.ajax({
                    url: '?',
                    data: params
                }).done(function(data) {
                    if ($.isArray(data) && data.length > 0) {
                        fillTable(data);
                        fillCharts(data);
                    } else {
                        Materialize.toast('没有查询到结果:(=', 3000);
                    }
                });
            }
        });
    });
})(window, jQuery);