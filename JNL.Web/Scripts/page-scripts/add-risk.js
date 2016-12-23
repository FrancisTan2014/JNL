(function(window, $) {
    $(function() {
        page.init();
        page.bindEvents();
    });

    window.page = {
        init: function () {
            var _this = this;

            
            $('#dtBox').DateTimePicker();

            // input intelligence
            $.inputIntelligence('#RespondStaffId', _this.staffItelligenceConfig);
            $.inputIntelligence('#ReportStaffId', _this.staffItelligenceConfig);
            $.inputIntelligence('#RiskSummaryId', _this.riskIntelligenceConfig);

            // select drop down
            common.loadDictionaries(_this.selectConfigs);

            // line select
            $.initSelect({
                selectors: '#OccurrenceLineId',
                ajaxUrl: '/Common/GetList',
                textField: 'Name',
                valueField: 'Id',
                getAjaxParams: function () { return { TableName: 'Line' }; },
                afterBuilt: function ($select) { $select.material_select(); }
            });
            
            var textAreaRules = {
                required: true,
                minlength: 10,
                maxlength: 8000
            };
            var textAreaMsgs = {
                required: '此项不能为空',
                minlength: '请输入不少于十个字',
                maxlength: '不能超过八千字'
            };
            $(".form-validate").validate({
                rules: {
                    RiskDetails: textAreaRules,
                    Summary: textAreaRules,
                    ReportStaff: { required: true },
                    RespondStaff: { required: true },
                    OccurrenceTime: { required: true }
                },
                //For custom messages
                messages: {
                    ReportStaff: { required: '提报人不能为空' },
                    RespondStaff: { required: '责任人不能为空' },
                    OccurrenceTime: { required: '时间不能为空' },
                    Summary: textAreaMsgs,
                    RiskDetails: textAreaMsgs,
                },
                errorElement: 'div',
                errorPlacement: function (error, element) {
                    var placement = $(element).data('error');
                    if (placement) {
                        $(placement).append(error);
                    } else {
                        error.insertAfter(element);
                    }
                }
            });
        },

        selectConfigs: [
            { type: 8, selector: '#LocoServiceTypeId', value: 'Id' },
            { type: 9, selector: '#WeatherId', value: 'Id' }
        ],

        staffItelligenceConfig: {
            ajaxUrl: '/Common/GetList',
            getAjaxParams: function(input) {
                return {
                    TableName: 'ViewStaff',
                    Fields: 'Id###WorkId###SalaryId###Name###Department',
                    PageIndex: 1,
                    PageSize: 50,
                    Conditions: '(SalaryId LIKE \'{0}%\' OR WorkId LIKE \'{0}%\' OR Name LIKE \'{0}%\')'.format(input)
                };
            },
            buildDropdownItem: function(data) {
                return '{0} | {1} | {2}'.format(data.SalaryId, data.Name, data.WorkId);
            },
            afterSelected: function (data, $input) {
                if ($input.prop('name') === 'ReportStaff') {
                    // 提报人
                    $input.prev().val(data.Id)
                        .parent().next().find('input').val(data.Department || '未知部门');
                } else {
                    // 责任人
                    var text = '{0} | {1} | {2}'.format(data.SalaryId, data.Name, data.WorkId);
                    page.fillRespInput(data.Id, text, data.Department, data.Name);
                    $('[name=RespondStaffId]').val(data.Id);
                }
            }
        },

        riskIntelligenceConfig: {
            ajaxUrl: '/Common/GetList',
            getAjaxParams: function(input) {
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
                $input.prev().val(data.Id);

                var riskLevels = $('#RiskLevels').val() || '';
                var needWrite = riskLevels.split(',').contains(data.SecondLevelId);
                // 根据风险概述判断是否需要填写整改信息
                $('input[name=NeedWriteFixDesc]').val(needWrite);
            }
        },

        bindEvents: function() {
            var _this = this;

            // 点击添加责任人
            $('.add-resp').on('click', function() {
                var $input = $('#RespondStaffId'),
                    completed = $input.data('complete'),
                    staffId = $input.data('staffid'),
                    text = $input.data('text'),
                    depart = $input.data('depart'),
                    name = $input.data('name'),
                    $addBtn = $(this);

                if (completed) {
                    if (!_this.respExsits(staffId)) {
                        var $resName = $('<span class="resp-name" title="点击修改"></span>');
                        $resName.data({
                            staffid: staffId,
                            text: text,
                            depart: depart
                        }).text(name);

                        $resName.on('click', function () {
                            _this.respNameClick($(this));
                        });

                        $resName.insertAfter($addBtn);
                    }

                    _this.cleanRespInput();
                    $('.resp-name.active').removeClass('active');
                } 
            });

            // 责任人被更改时
            $('#RespondStaffId').on('keydown', function () {

                var staffId = $(this).data('staffid'),
                    completed = $(this).data('complete');

                if (completed) {
                    // 删除加号后面的名称
                    $('.resp-name').each(function () {
                        if ($(this).data('staffid') == staffId) {
                            $(this).remove();
                        }
                    });

                    _this.cleanRespInput();
                }
            });

            $('#OccurrenceLineId').on('change', function() {
                var lineId = $(this).val();

                $.initSelect({
                    selectors: ['#FirstStationId', '#LastStationId'],
                    ajaxUrl: '/Common/GetList',
                    textField: 'StationName',
                    valueField: 'StationId',
                    getAjaxParams: function() {
                        return {
                            TableName: 'ViewLine',
                            Conditions: 'Id=' + lineId,
                            OrderField: 'Sort',
                            Desending: true
                        };
                    },
                    beforeBuilt: function($select) {
                        $select.find('option:not(:first)').remove();
                    },
                    afterBuilt: function($select) {
                        $select.material_select();
                    }
                });
            });

            // 选择终点站（确保终点站与起点站挨着）
            $('#LastStationId').on('change.neighbor', function () {
                _this.lastStationChange();
            });

            $('#Switch').on('change', function() {
                $('#Visible').val($(this).is(':checked'));
            });

            $('#btnSave').on('click', function () {
                _this.save();
            });
        },

        lastStationChange: function() {
            var $firstStation = $('#FirstStationId'),
                startId = $firstStation.val(),
                $selectedOption = $firstStation.find('option[value=' + startId + ']'),
                prevId = $selectedOption.prev().val() || -2,
                nextId = $selectedOption.next().val() || -2,
                $lastSelect = $('#LastStationId'),
                lastId = $lastSelect.val(),
                _this = this;

            if (![prevId, startId, nextId].contains(lastId)) {
                Materialize.toast('终点站必须是与起点站相邻最近的那一个车站，请重新选择:(=', 3000);

                $lastSelect.off('change.neighbor');
                $lastSelect.prev().find('li:first').click();
                $lastSelect.on('change.neighbor', function() {
                    _this.lastStationChange();
                });
            }
        },

        respExsits: function(staffid) {
            
            var exsits = false;
            $('.resp-name').each(function() {
                if ($(this).data('staffid') == staffid) {
                    exsits = true;
                    return false;
                }
            });

            return exsits;
        },

        respNameClick: function ($resp) {

            $resp.addClass('active').siblings().removeClass('active');
            this.fillRespInput(
                $resp.data('staffid'),
                $resp.data('text'),
                $resp.data('depart'),
                $resp.data('name')
                );
        },

        fillRespInput: function(staffid, text, depart, name) {
            var $input = $('#RespondStaffId'),
                $depart = $input.parent().next().find('input');

            $input.data({
                complete: true,
                staffid: staffid,
                text: text,
                depart: depart,
                name: name
            }).val(text);

            $depart.val(depart || '未知部门');
        },

        cleanRespInput: function() {
            var $input = $('#RespondStaffId'),
                $depart = $input.parent().next().find('input');

            $input.data({
                complete: false,
                staffid: 0,
                text: '',
                depart: '',
                name: ''
            }).val('');

            $depart.val('');
        },

        save: function() {
            if ($('.edit-form').valid()) {
                var riskInfo = common.formJsonfiy('.edit-form');
                if (riskInfo.ReportStaffId <= 0) {
                    Materialize.toast('提报人没有从提示下拉框中选择，不能提交', 3000);
                    return;
                }

                var respId = $('[name=RespondStaffId]').val();
                var respStaffIds = [];
                if (respId > 0) {
                    respStaffIds.push(respId);
                }

                $('.resp-name').each(function () {
                    var staffid = $(this).data('staffid');
                    if (staffid != respId) {
                        respStaffIds.push(staffid);
                    }
                });

                if (respStaffIds.length === 0) {
                    //Materialize.toast('责任人没有从提示下拉框中选择，不能提交', 3000);

                    // @FrancisTan 2016-12-11 修改提示信息內容
                    Materialize.toast('请输入：责任人不清楚待查找确定', 3000);
                    return;
                }

                var valid = true;
                $('select.initialized').each(function () {
                    if ($(this).prop('name') !== 'LastStationId') {
                        var error = '请选择' + $(this).find('option:first').text();
                        if ($(this).val() <= 0) {
                            valid = false;

                            Materialize.toast(error, 3000);
                            return false;
                        }
                    }
                });

                if (!valid) {
                    return;
                }

                if (riskInfo.RiskSummaryId <= 0) {
                    Materialize.toast('风险概述信息没有从提示下拉框中选择，不能提交', 3000);
                    return;
                }

                if (confirm('确认保存？')) {
                    // save
                    var forbidden = common.submitForbidden('#btnSave', '正在保存');
                    common.ajax({
                        url: '/Risk/AddRisk',
                        data: {
                            responds: respStaffIds.join('###'),
                            risk: JSON.stringify(riskInfo)
                        }
                    }).done(function (res) {
                        if (res.code == 100) {
                            Materialize.toast('保存成功', 1000, '', function () {
                                location.reload();
                            });

                            return;
                        }

                        forbidden.enabled();
                        Materialize.toast('保存失败，请稍后重试！', 3000);
                    });
                }

            } else {
                Materialize.toast('请完善信息！！！', 3000);
            }
        }
    };
})(window, jQuery);