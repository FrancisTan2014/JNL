(function (window, $) {
    // 初始化部门下拉选择框
    function initDeparts(selectors) {
        $.initSelect({
            selectors: selectors,
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
    }

    // 修改指标点击事件
    window.editClick = function(dom) {
        var $this = $(dom),
            quotaId = $this.data('id'),
            ammount = $this.data('ammount');

        window.editStatus.isAdding = false;
        window.editStatus.quotaId = quotaId;
        window.editStatus.$editElem = $this;

        $('#editTitle').text('修改指标');
        $('#QuotaAmmount').val(ammount);
        $('#targetStaff').hide();

        $('#modal').openModal({ dismissible: false, opacity: 0.5 });
        $('#QuotaAmmount').focus();
    }

    // 初始化表格
    var table = $.commonTable('.data-table', {
        columns: ['Id', 'Name', 'Department', 'QuotaType', 'QuotaAmmount', 'UpdateTime', 'Id'],
        builds: [
            {
                targets: [3],
                onCreateCell: function(columnValue) {
                    return '<td>风险信息录入条数指标</td>';
                }
            },
            {
                targets: [6],
                onCreateCell: function (columnValue, data) {
                    return '<td><a href="javascript:(0);" onclick="window.editClick(this);" data-id="{0}" data-ammount="{1}" title="点击修改"><i class="small mdi-editor-border-color"></i></a></td>'.format(columnValue, data.QuotaAmmount);
                }
            }],
        ajaxParams: {
            TableName: 'ViewQuota',
            OrderField: 'UpdateTime',
            Desending: true,
            PageSize: 20,
            PageIndex: 1
        },
        getConditions: function () {
            var conditions = [];
            $('#searchBox').find('select.initialized,input.datepicker').each(function () {
                var $this = $(this),
                    value = $this.val();

                if (value != -1 && value != '' && value != undefined) {
                    var con = $this.data('condition').replace('{{value}}', value);
                    conditions.push(con);
                }
            });

            return conditions;
        }
    });

    // 此对象用于在添加指标与修改指标之间切换状态
    window.editStatus = {
        isAdding: true,
        quotaId: 0,
        staffId: 0,
        $editElem: null
    };

    // 绑定事件
    function bindEvents() {
        // 人员输入框输入事件
        $('#staffName').on('keydown', function() {
            if (window.editStatus.staffId > 0) {
                window.editStatus.staffId = 0;
                $(this).val('');
            } 
        });

        // 限定指标量化数只能输入数字
        $('#QuotaAmmount').on('keyup', function (e) {
            var value = $(this).val();
            if (isNaN(value)) {
                $(this).val('');
            }
        });

        // 添加指标
        $('#btnAdd').on('click', function() {
            $('#editTitle').text('添加指标');
            window.editStatus.isAdding = true;
            window.editStatus.staffId = 0;
            $('#staffName').val('');
            $('#QuotaAmmount').val('');
            $('#targetStaff').show();

            $('#modal').openModal({ dismissible: false, opacity: 0.5 });
        });

        // 保存指标
        $('#btnSave').on('click', function () {
            var ammount = parseInt($('#QuotaAmmount').val());
            if (ammount <= 0) {
                Materialize.toast('请输入有效的指标量化数:(=', 3000);
                return;
            }

            if (window.editStatus.isAdding) {
                if (window.editStatus.staffId <= 0) {
                    Materialize.toast('请指定您设置此指标的目标人员:(=', 3000);
                    return;
                }

                common.ajax({
                    url: '/Admin/AddQuota',
                    data: {
                        staffId: window.editStatus.staffId,
                        ammount: ammount
                    }
                }).done(function(res) {
                    if (res.code == 100) {
                        Materialize.toast('添加成功:(=', 1000, '', function() {
                            window.location.reload();
                        });
                    } else {
                        Materialize.toast('添加失败，请稍后重试:(=', 3000);
                    }
                });
            } else {
                common.ajax({
                    url: '/Admin/UpdateQuota',
                    data: {
                        quotaId: window.editStatus.quotaId,
                        ammount: ammount
                    }
                }).done(function(res) {
                    if (res.code == 100) {
                        Materialize.toast('修改成功:(=', 1000, '', function () {
                            window.editStatus.$editElem.parent().prev().prev().text(ammount);
                        });
                    } else {
                        Materialize.toast('修改失败，请稍后重试:(=', 3000);
                    }
                });
            }
        });
    }

    $(function () {
        initDeparts(['#searchReport', '#searchRespond']);

        // 初始化天气、列车类别下拉选择框
        common.loadDictionaries([
            { type: 9, selector: '#searchWeather', value: 'Id' },
            { type: 8, selector: '#searchLocoType', value: 'Id' }
        ]);

        // 初始化时间选择器
        common.pickdate();

        // 初始化人员智能提示
        $.inputIntelligence('#staffName', {
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
                window.editStatus.staffId = data.Id;
                $input.val('{0} | {1} | {2}'.format(data.SalaryId, data.Name, data.WorkId));
            }
        });

        $('#btnSearch').on('click', function () {
            table.setPageIndex(1);
            table.loadData();
        });

        bindEvents();
    });
})(window, jQuery);