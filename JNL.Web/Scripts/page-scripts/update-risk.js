(function (window, $) {
    $(function () {
        page.bindEvents();
        page.init();

        setTimeout(function () {
            var defaultFirstStationId = $('#defaultFirstStationId').val();
            var $firstStationSelect = $('#FirstStationId');
            $firstStationSelect.find('option[value={0}]'.format(defaultFirstStationId)).prop('selected', true);
            $firstStationSelect.material_select();

            var defaultLastStationId = $('#defaultLastStationId').val();
            var $lastStationSelect = $('#LastStationId');
            $lastStationSelect.find('option[value={0}]'.format(defaultLastStationId)).prop('selected', true);
            $lastStationSelect.material_select();
        }, 500);
    });

    window.page = {
        init: function () {
            var _this = this;

            var defaultCurrenceTime = $('#defaultOccurrenceTime').val();
            $('#dtBox').DateTimePicker({ defaultDate: new Date(defaultCurrenceTime) });

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
                afterBuilt: function ($select) {
                    var defaultLineId = $('#defaultOccurrenceLineId').val();
                    $select.find('option[value={0}]'.format(defaultLineId)).prop('selected', true);
                    $select.change();

                    $select.material_select();
                }
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
            { type: 8, selector: '#LocoServiceTypeId', value: 'Id', selected: $('#defaultLocoServiceTypeId').val() },
            { type: 9, selector: '#WeatherId', value: 'Id', selected: $('#defaultWeatherId').val() }
        ],

        staffItelligenceConfig: {
            ajaxUrl: '/Common/GetList',
            getAjaxParams: function (input) {
                return {
                    TableName: 'ViewStaff',
                    Fields: 'Id###WorkId###SalaryId###Name###Department',
                    PageIndex: 1,
                    PageSize: 50,
                    Conditions: '(SalaryId LIKE \'{0}%\' OR WorkId LIKE \'{0}%\' OR Name LIKE \'{0}%\')'.format(input)
                };
            },
            buildDropdownItem: function (data) {
                return '{0} | {1} | {2}'.format(data.SalaryId, data.Name, data.WorkId);
            },
            afterSelected: function (data, $input) {
                $input.prev().val(data.Id)
                    .parent().next().find('input').val(data.Department || '未知部门');
            }
        },

        riskIntelligenceConfig: {
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
                $input.prev().val(data.Id);

                var riskLevels = $('#RiskLevels').val() || '';
                var needWrite = riskLevels.split(',').contains(data.SecondLevelId);
                // 根据风险概述判断是否需要填写整改信息
                $('input[name=NeedWriteFixDesc]').val(needWrite);
            }
        },

        btnToggleDisbled: function () {
            var disbled = $('.btn-group button:first').prop('disabled');
            $('.btn-group button').prop('disabled', !disbled);
        },

        bindEvents: function () {
            var _this = this;

            $('.toggle-show').on('click', function () {
                var value = $(this).data('value');
                $(this).addClass('active').siblings().removeClass('active');
                $(this).siblings('input[type=hidden]').val(value);
            });

            $('#OccurrenceLineId').on('change', function () {
                var lineId = $(this).val();

                $.initSelect({
                    selectors: ['#FirstStationId', '#LastStationId'],
                    ajaxUrl: '/Common/GetList',
                    textField: 'StationName',
                    valueField: 'StationId',
                    getAjaxParams: function () {
                        return {
                            TableName: 'ViewLine',
                            Conditions: 'Id=' + lineId,
                            OrderField: 'Sort',
                            Desending: true
                        };
                    },
                    beforeBuilt: function ($select) {
                        $select.find('option:not(:first)').remove();
                    },
                    afterBuilt: function ($select) {
                        $select.material_select();
                    }
                });
            });

            $('.edit-shadow').on('click', function () {
                Materialize.toast('当前为不可编辑状态，请点击修改', 3000);
            });

            $('#Switch').on('change', function () {
                $('#Visible').val($(this).is(':checked'));
            });

            $('#btnEdit').on('click', function () {
                $('.edit-shadow').hide();
                $(this).hide();
            });

            $('#btnDelete').on('click', function () {
                $.confirm('提示', '您确定要删除此风险信息吗？', function () {
                    var id = $('input[name=Id]').val();

                    _this.btnToggleDisbled();
                    common.ajax({
                        url: '/Risk/DeleteRisk',
                        data: { id: id }
                    }).done(function (res) {
                        _this.btnToggleDisbled();
                        if (res.code == 100) {
                            Materialize.toast(res.msg, 1000, '', function () {
                                history.back();
                            });
                            return;
                        }

                        Materialize.toast(res.msg, 3000);
                    }).error(function () {
                        _this.btnToggleDisbled();
                    });
                });
            });

            $('#btnPass,#btnVote').on('click', function () {
                var status = $(this).data('pass');
                _this.vote(status);
            });

            $('#btnBack').on('click', function () {
                history.back();
            });
        },

        validateFormAndGetJson: function () {
            if ($('.edit-form').valid()) {
                var riskInfo = common.formJsonfiy('.edit-form');
                if (riskInfo.ReportStaffId <= 0) {
                    Materialize.toast('提报人没有从提示下拉框中选择，不能提交', 3000);
                    return null;
                }
                if (riskInfo.RespondStaffId <= 0) {
                    Materialize.toast('责任人没有从提示下拉框中选择，不能提交', 3000);
                    return null;
                }

                var valid = true;
                $('select.initialized').each(function () {
                    var name = $(this).prop('name');
                    if (name != 'LastStationId') {
                        var error = '请选择' + $(this).find('option:first').text();
                        if ($(this).val() <= 0) {
                            valid = false;

                            Materialize.toast(error, 3000);
                            return false;
                        }
                    }
                });

                if (!valid) {
                    return null;
                }

                if (riskInfo.RiskSummaryId <= 0) {
                    Materialize.toast('风险概述信息没有从提示下拉框中选择，不能提交', 3000);
                    return null;
                }

                return riskInfo;
            } else {
                Materialize.toast('请完善信息！！！', 3000);
                return null;
            }
        },

        vote: function (status) {
            var _this = this;
            var riskInfo = _this.validateFormAndGetJson();

            if (riskInfo) {
                riskInfo.VerifyStatus = status;
                _this.btnToggleDisbled();
                common.ajax({
                    url: '/Risk/UpdateRisk',
                    data: {
                        responds: riskInfo.RespondStaffId,
                        risk: JSON.stringify(riskInfo)
                    }
                }).done(function (res) {
                    _this.btnToggleDisbled();
                    if (res.code == 100) {
                        Materialize.toast(res.msg, 1000, '', function () {
                            history.back();
                        });
                        return;
                    }

                    Materialize.toast(res.msg, 3000);
                }).error(function (e) {
                    console.info(e);
                    _this.btnToggleDisbled();
                });
            }
        }
    };
})(window, jQuery);