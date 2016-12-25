(function(window, $) {

    $(function() {
        page.init();
        page.bindEvents();
    });

    window.page = {
        
        lineId: $('.edit-form').data('id'),

        // {  Id	Name	StationId	StationName	Sort }
        stations: [],
        
        init: function() {
            var _this = this;

            if (_this.lineId > 0) {
                // 加载线路信息
                common.ajax({
                    url: '/Common/GetList',
                    data: {
                        TableName: 'Line',
                        Conditions: 'Id=' + _this.lineId
                    }
                }).done(function(res) {
                    if (res.code == 108) {
                        $('[name=Name]').val(res.data[0].Name);
                    }
                });

                // 加载车站信息
                common.ajax({
                    url: '/Common/GetList',
                    data: {
                        TableName: 'ViewLine',
                        OrderField: 'Sort',
                        Conditions: 'Id=' + _this.lineId
                    }
                }).done(function(res) {
                    if (res.code == 108) {
                        _this.stations = res.data;

                        _this.renderStations(res.data);
                    }
                });
            }
        },

        bindEvents: function() {
            var _this = this;

            // 添加车站输入框获取焦点事件
            // 策略：为了操作方便，每次获取焦点时将文本清空
            $('#addStationName').on('focus', function() {
                $(this).val('');
            });

            // 点击添加车站
            $('#btnAddStation').on('click', function() {
                _this.addStationClick();
            });

            // 点击保存
            $('#btnSave').on('click', function() {
                _this.save();
            });
        },

        // 根据指定数据渲染车站
        renderStations: function (stations) {
            var _this = this,
                icon = _this.getIconHtml(),
                $stationGroup = $('.station-group'),
                isInit = $stationGroup.children().length === 0;

            if (!isInit) {
                $stationGroup.append(icon);
            }

            stations = stations instanceof Array ? stations : [stations];
            stations.forEach(function(value, index) {
                if (index > 0) {
                    $stationGroup.append(icon);
                }

                var currStation = stations[index],
                    $station = _this.buildStationView(currStation);

                // 为添加的车站绑定事件
                $station.on('click', function() {
                    if (confirm('您确定要将此车站从线路中删除吗？')) {
                        _this.removeStation(currStation);
                    }
                });

                $stationGroup.append($station);
            });
        },

        // 建立车站展示视图Dom，返回新建的jQuery对象
        buildStationView: function(station) {
            return $('<span class="station" data-id={0} title="点击删除">{1}</span>'.format(station.StationId, station.StationName));
        },

        // 获取车站之间连接的icon元素的html代码
        getIconHtml: function() {
            return '<i class="mdi-image-navigate-next"></i>';
        },

        // 移除车站
        removeStation: function (station) {
            var _this = this,
                $stationGroup = $('.station-group');

            // 将车站从stations数组中移除
            _this.stations.remove(station, function(v1, v2) { return v1.StationId == v2.StationId });

            // 将车站从视图中移除
            var $station;
            $stationGroup.find('.station').each(function() {
                if ($(this).data('id') == station.StationId) {
                    $station = $(this);
                }
            });

            if ($station.index() === 0) {
                $station.next().remove();
            }

            $station.prev().remove();
            $station.remove();
        },

        // 点击添加车站
        addStationClick: function() {
            var _this = this,
                name = $('#addStationName').val();
            
            if (name) {
                common.ajax({
                    url: '/Common/GetList',
                    data: {
                        TableName: 'Station',
                        Conditions: '[Name]=\'{0}\''.format(name)
                    }
                }).done(function(res) {
                    if (res.code == 108 && res.data.length > 0) {
                        var stationModel = {
                            StationId: res.data[0].Id,
                            StationName: name
                        };

                        if (!_this.isStationExists(stationModel)) {
                            _this.renderStations(stationModel);
                            _this.stations.push(stationModel);
                        } else {
                            Materialize.toast('线路中已存在此车站，不能重复添加:(=', 3000);
                        }
                    } else {
                        Materialize.toast('添加失败，不存在此车站:(=', 3000);
                    }
                });
            }
        },

        // 判断指定车站是否已存在于当前线路中
        isStationExists: function(station) {
            var _this = this;
            return _this.stations.contains(function(elem) {
                return elem.StationId == station.StationId;
            });
        },

        // 保存线路信息
        save: function () {
            var _this = this;

            // 线路名称不能为空
            var lineName = $('[name=Name]').val();
            if (!lineName) {
                Materialize.toast('线路名称不能为空:(=', 3000);
                $('[name=Name]').focus();
                return;
            }

            // 车站数量必须多于2个
            if (_this.stations.length < 2) {
                Materialize.toast('保存失败，车站数量过少:(=');
                return;
            }

            var stationIds = _this.stations.select(function(elem) { return elem.StationId; });
            common.ajax({
                url: '?',
                data: {
                    lineId: _this.lineId,
                    lineName: lineName,
                    firstStation: _this.stations.first().StationName,
                    lastStation: _this.stations.last().StationName,
                    stationIds: stationIds.join('###')
                }
            }).done(function(res) {
                if (res.code == 100) {
                    Materialize.toast('操作成功，即将返回:(=', 1000, '', function() {
                        history.back();
                    });
                } else {
                    Materialize.toast('操作失败，请稍后重试:(=', 3000);
                }
            });
        }

    };

})(window, jQuery);