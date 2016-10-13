(function (window, $) {
    $(function () {
        page.init();
        page.bindEvents();

        $('#btnSearch').click();
    });

    window.page = {
        commonTable: null,
        commonAjaxUrl: '/Common/GetList',

        init: function () {
            var _this = this;

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
        },

        bindEvents: function () {
            var _this = this;
            $('#btnSearch').on('click', function () {
                var year = $('#searchYear').val();
                var depart = $('#searchDepart').val();

                common.ajax({
                    url: '?',
                    data: {
                        year: year,
                        departId: depart
                    }
                }).done(function (data) {
                    var chartBox = $('.chart-box')[0];
                    _this.buildChart(chartBox, data);
                });
            });
        },

        buildChart: function(dom, data) {
            var legendData = data.ammounts.select(function (item) { return item.name; });
            legendData.push(data.rotes.name);

            var series = data.ammounts.select(function (item) {
                return {
                    name: item.name,
                    type: 'line',
                    data: item.value
                };
            });
            series.push({
                name: data.rotes.name,
                type: 'line',
                yAxisIndex: 1,
                data: data.rotes.value.select(function (item) { return item.toFixed(4) })
            });
            
            var chart = echarts.init(dom);
            var options = {
                //backgroundColor: '#2bbbad',
                color: ['#d50000', '#6200ea', '#004d40', '#2196f3', '#f9a825'],
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow'
                    }
                },
                legend: {
                    data: legendData
                },
                xAxis: [
                    {
                        type: 'category',
                        splitLine: {
                            show: false,
                        },
                        data: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                        axisTick: {
                            alignWithLabel: true
                        }
                    }
                ],

                yAxis: [
                    {
                        type: 'value',
                        name: '数量',
                        splitLine: {
                            show: true,
                        },
                        axisLabel: {
                            formatter: '{value} 个'
                        }
                    },
                    {
                        type: 'value',
                        name: '占比',
                        splitLine: {
                            show: false,
                        },
                        axisLabel: {
                            formatter: '{value} %'
                        }
                    }
                ],
                series: series
            };

            chart.setOption(options);
        }
    };
})(window, jQuery);