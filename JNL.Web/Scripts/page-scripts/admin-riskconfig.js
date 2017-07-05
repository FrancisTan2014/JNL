(function ($, window) {
    $(function () {
        window.page.init();
    });

    window.page = {
        configType: 1,

        init: function () {
            var _this = this;

            // 加载数据
            _this.load();

            // 输入框智能提示
            $.inputIntelligence('#keyWords', {
                ajaxUrl: '/Common/GetList',
                getAjaxParams: function (input) {
                    return {
                        TableName: 'RiskSummary',
                        Fields: 'Id###Description###TopestName###SecondLevelId###SecondLevelName',
                        PageIndex: 1,
                        PageSize: 50,
                        Conditions: 'Description LIKE \'%{0}%\' AND IsBottom=1'.format(input)
                    };
                },
                buildDropdownItem: function (data) {
                    return '<font style="color: #f50057">【{0}-{1}】</font>{2}'.format(data.TopestName, data.SecondLevelName, data.Description);
                },
                afterSelected: function (data, $input) {
                    _this.addConfig(data);
                    $input.val('');
                }
            });

            // 配置项类型选项改变
            $('#configType').on('change',
                function () {
                    var val = $(this).val();
                    if (val != _this.configType) {
                        _this.configType = $(this).val();
                        _this.load();
                    }
                });
        },

        load: function () {
            var _this = this;
            common.ajax({
                url: '/Common/GetList',
                data: {
                    TableName: 'ViewAdminConfig',
                    Conditions: 'ConfigType=' + _this.configType
                }
            }).done(function (res) {
                if (res.data) {
                    $('#container').empty();
                    res.data.forEach(function (val) {
                        _this.addCard(val);
                    });
                }
            });
        },

        render: function (data) {
            return $('#template').html()
                .replace('{{title}}', '【{0}-{1}】'.format(data.TopestName, data.SecondLevelName))
                .replace('{{desc}}', data.Description)
                .replace('{{id}}', data.TargetId || data.Id);
        },

        addConfig: function (data) {
            var _this = this;
            // 判断所选项是否已存在
            common.ajax({
                url: '/Admin/ConfigExists',
                data: {
                    type: _this.configType,
                    target: data.Id
                }
            }).done(function (res) {
                if (res.code == 112) {
                    Materialize.toast('该项已存在', 3000);
                } else {
                    // 向数据库中添加配置项
                    common.ajax({
                        url: '/Common/InsertData',
                        data: {
                            json: JSON.stringify({ ConfigType: _this.configType, TargetId: data.Id }),
                            target: 'AdminConfig'
                        }
                    }).done(function(r) {
                        if (r.code == 100) {
                            _this.addCard(data);
                        }
                    });
                }
            });
        },

        addCard: function (data) {
            var _this = this;
            var html = _this.render(data);
            var $card = $(html);
            $card.find('.remove').on('click', function () {
                _this.removeConfig.apply(this);
            });

            $('#container').append($card);
        },

        removeConfig: function () {
            var _this = window.page,
                btn = this;
            if (confirm('您确定要删除此项吗？')) {
                var target = $(this).data('id');
                common.ajax({
                    url: '/Admin/DeleteConfig',
                    data: {
                        type: _this.configType,
                        target: target
                    }
                }).done(function(res) {
                    if (res.code == 100) {
                        Materialize.toast('删除成功!', 3000);
                        $(btn).parents('.col').remove();
                    } else {
                        Materialize.toast('操作失败，请稍后重试！', 3000);
                    }
                });
            }
        }
    };
})(jQuery, window);