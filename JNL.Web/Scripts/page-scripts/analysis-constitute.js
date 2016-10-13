(function (window, $) {
    $(function () {
        common.pickdate();

        $.initSelect({
            selectors: ['#searchDepart'],
            ajaxUrl: '/Common/GetList',
            textField: 'Name',
            valueField: 'Id',
            getAjaxParams: function () {
                return { TableName: 'Department' };
            },
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
                    if ($.isArray(res) && res.length > 0) {
                        buildChart(res);
                    } else {
                        clearChart();
                        Materialize.toast('没有查询到结果！', 3000);
                    }
                });
            }
        });

        $('#btnSearch').click();
    });

    // 获取异步请求参数
    function getAjaxParams() {
        var depart = $('#searchDepart').val();
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
            departId: depart, start: start, end: end
        };
    }

    function buildChart(data) {
        var title = $('#searchDepart>option:selected').text() + '安全问题构成分析';
        var legends = data.select(function (item) { return item.name });

        var chartBox = $('.chart-box')[0];
        var chart = echarts.init(chartBox);

        chart.setOption({
            title: {
                text: title,
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b}：占比({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: legends
            },
            series: [
                {
                    name: '绝对分布占比',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: data,
                    itemStyle: {
                        emphasis: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ]
        });
    }

    function clearChart() {
        $('.chart-box').empty();
    }
})(window, jQuery);