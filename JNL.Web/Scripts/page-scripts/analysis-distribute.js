(function (window, $) {
    $(function () {
        common.pickdate();

        $('#btnSearch').on('click', function () {
            clearTable();
            clearCharts();

            var params = getAjaxParams();
            if (params) {
                common.ajax({
                    url: '?',
                    data: params
                }).done(function (res) {
                    if ($.isArray(res)) {
                        switch (+params.type) {
                            case 1: absuloteCount(res); break;
                            case 2: absulotePercent(res); break;
                            case 3: relateCount(res); break;
                            case 4: relateScore(res); break;
                        }
                    } else {
                        Materialize.toast('没有查询到结果！', 3000);
                    }
                });
            }
        });
        
        $('#btnSearch').click();
    });

    window.page = {};
    window.page.multi = new Multiselect('searchDepart', {
        placeholder: '责任部门',
        textKey: 'Name',
        valueKey: 'Id',
        loadData: function(callback, multiObj) {
            common.ajax({
                url: '/Common/GetList',
                data: {
                    TableName: 'Department'
                }
            }).done(function(res) {
                var data = [];
                if (res.code == 108) {
                    data = res.data;
                }

                callback(data, multiObj);
            });
        }
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

        var departs = window.page.multi.selected.select(function (item) { return item.value }).join();

        return {
            type: type, start: start, end: end, departs: departs
        };
    }

    /**********************四大业务方法************************/
    // 绝对数量分布
    function absuloteCount(data) {
        initRows(7);

        initColumnTitle(['风险类别', '红线', '甲Ⅰ', '甲Ⅱ', '乙', '丙', '信息', '合计']);
        data.forEach(function (item) {
            fillColumn(item, ['Name', 'Hx', 'Jy', 'Je', 'Yi', 'Bi', 'Xx', 'Hj']);
        });
    }

    // 绝对占比分布
    function absulotePercent(data) {
        initRows(7);

        var columnTitle = ['风险类别', '红线', '甲Ⅰ', '甲Ⅱ', '乙', '丙', '信息', '合计'];
        initColumnTitle(columnTitle);
        data.forEach(function (item) {
            fillColumn(item, ['Name', 'Hx', 'Jy', 'Je', 'Yi', 'Bi', 'Xx', 'Hj']);
        });

        var departs = data.select(function (item) { return item.Name });
        var hxData = data.select(function (item) { return { name: item.Name, value: item.Hx } });
        var jyData = data.select(function (item) { return { name: item.Name, value: item.Jy } });
        var jeData = data.select(function (item) { return { name: item.Name, value: item.Je } });
        var yiData = data.select(function (item) { return { name: item.Name, value: item.Yi } });
        var biData = data.select(function (item) { return { name: item.Name, value: item.Bi } });
        var xxData = data.select(function (item) { return { name: item.Name, value: item.Xx } });
        var hjData = data.select(function (item) { return { name: item.Name, value: item.Hj } });

        var $container = $('.chart-container');
        var dataList = [hxData, jyData, jeData, yiData, biData, xxData, hjData];

        dataList.forEach(function(data, i) {
            var dom = $('<div class="chart-canvas"></div>')[0];
            var title = '{0}类绝对分布占比'.format(columnTitle[i+1]);

            $container.append(dom);
            buildPie(dom, title, departs, data);
        });
    }

    // 相对数量分布
    function relateCount(data) {

        initRows(2);

        // 初始化表头
        var titles = ['对比项', '自查均量', '外检均量'];
        initColumnTitle(titles);

        data.forEach(function(item) {
            fillColumn(item, ['name', 'self', 'other']);
        });

        var departs = data.select(function (item) { return item.name });
        var selves = data.select(function (item) { return item.self });
        var others = data.select(function (item) { return item.other });

        var legends = ['自查', '外检'];
        var series = [
            createBarSeries(legends[0], selves),
            createBarSeries(legends[1], others)
        ];

        var chartDom = $('<div class="chart-canvas"></div>')[0];
        $('.chart-container').append(chartDom);

        buildBar(chartDom, legends, departs, series);
    }

    // 相对分值分布
    function relateScore(data) {
        initRows(6);

        var columnTitle = ['风险类别', '红线', '甲Ⅰ', '甲Ⅱ', '乙', '丙', '合计'];
        initColumnTitle(columnTitle);
        data.forEach(function(item) {
            fillColumn(item, ['Name', 'Hx', 'Jy', 'Je', 'Yi', 'Bi', 'Hj'])
        });
    }

    /**********************图表相关操作************************/
    // 清空图表
    function clearCharts() {
        $('.chart-container').empty();
    }

    // 创建饼状图
    function buildPie(dom, title, itemTexts, data) {
        var chart = echarts.init(dom);
        chart.setOption({
            title: {
                text: title,
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b}：{c}"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: itemTexts
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

    // 创建柱状图
    function buildBar(dom, legends, yAxis, series) {
        var chart = echarts.init(dom);
        chart.setOption({
            tooltip : {
                trigger: 'axis',
                axisPointer : {            
                    type : 'shadow'        
                }
            },
            legend: {
                data: legends
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis:  {
                type: 'value'
            },
            yAxis: {
                type: 'category',
                data: yAxis
            },
            series: series
        });
    }

    // 创建画柱状态所需要的数据序列
    function createBarSeries(name, data) {
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
            data: data
        };
    }

    /**********************表格相关操作************************/
    // 清空表格
    function clearTable() {
        $('.data-table').find('thead>tr').empty().end()
            .find('tbody').empty();
    }

    // 初始化数据行
    function initRows(rowCount) {
        var $tbody = $('.data-table').find('tbody');
        for (var i = 0; i < rowCount; i++) {
            $tbody.append('<tr></tr>');
        }
    }

    // 初始化列标题
    function initColumnTitle(titles) {
        var indexs = [];
        for (var i = 0; i < titles.length; i++) {
            indexs.push(i);
        }

        fillColumn(titles, indexs);
    }

    // 填充数据列
    function fillColumn(obj, indexs) {
        $('.data-table>thead>tr').append('<th>{0}</th>'.format(obj[indexs[0]]));

        var $rows = $('.data-table>tbody>tr');
        var type = $('#searchType').val();
        for (var i = 1; i < indexs.length; i++) {
            var value = obj[indexs[i]];
            var text = value;

            if (type == 2) {
                text = dealNumber(value);
            }

            $rows.eq(i - 1).append('<td>{0}</td>'.format(text));
        }
    }

    // 将小数转换为0.00%的形式
    function dealNumber(value) {
        if (!isNaN(parseFloat(value))) {
            return (value * 100).toFixed(2) + '%';
        }

        return value;
    }
})(window, jQuery);