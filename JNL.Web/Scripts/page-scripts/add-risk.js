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
            afterSelected: function(data, $input) {
                $input.prev().val(data.Id)
                    .parent().next().find('input').val(data.Department || '未知部门');
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

            $('#Switch').on('change', function() {
                $('#Visible').val($(this).is(':checked'));
            });

            $('#btnSave').on('click', function () {
                _this.save();
            });
        },

        save: function() {
            if ($('.edit-form').valid()) {
                var riskInfo = common.formJsonfiy('.edit-form');
                if (riskInfo.ReportStaffId <= 0) {
                    Materialize.toast('提报人没有从提示下拉框中选择，不能提交', 3000);
                    return;
                }
                if (riskInfo.RespondStaffId <= 0) {
                    Materialize.toast('责任人没有从提示下拉框中选择，不能提交', 3000);
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

                // save
                var forbidden = common.submitForbidden('#btnSave', '正在保存');
                common.ajax({
                    url: '/Risk/AddRisk',
                    data: {
                        responds: riskInfo.RespondStaffId,
                        risk: JSON.stringify(riskInfo)
                    }
                }).done(function(res) {
                    if (res.code == 100) {
                        Materialize.toast('保存成功', 1000, '', function() {
                            location.reload();
                        });

                        return;
                    }

                    forbidden.enabled();
                    Materialize.toast('保存失败，请稍后重试！', 3000);
                });
            } else {
                Materialize.toast('请完善信息！！！', 3000);
            }
        }
    };
})(window, jQuery);