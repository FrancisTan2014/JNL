(function (window, $) {

    $(function () {
        page.init();
        page.bindEvents();
    });

    window.page = {
        table: null,
        $addModal: $('#addModal'),
        $editModel: $('#editModal'),

        // 初始化
        init: function() {
            
            var _this = this;

            // 初始化表格
            _this.table = $.commonTable('.data-table', {
                columns: ['Id', 'Name', 'Spell', 'AddTime', 'Id'],
                builds: [
                    {
                        targets: [4],
                        onCreateCell: function (columnValue, data) {
                            return '<td><a href="javascript:(0);" onclick="page.openEditModal({0}, \'{1}\', \'{2}\');" title="点击修改"><i class="small mdi-editor-border-color"></i></a></td>'.format(columnValue, data.Name, data.Spell);
                        }
                    }],
                ajaxParams: {
                    TableName: 'Station',
                    OrderField: 'AddTime',
                    Desending: true,
                    PageSize: 20,
                    PageIndex: 1
                },
                getConditions: function () {
                    var conditions = [];
                    $('#searchBox').find('select.initialized,input').each(function () {
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
        },

        // 绑定事件
        bindEvents: function () {

            var _this = this;

            // 点击查询
            $('#btnSearch').on('click', function () {
                _this.table.setPageIndex(1);
                _this.table.loadData();
            });

            // 点击添加车站
            $('#btnAdd').on('click', function() {
                _this.openAddModal();
            });

            // 点击保存车站信息
            $('.btn-save').on('click', function() {
                var $form = $(this).parents('.modal').find('form');
                _this.onSave($form);
            });
        },

        // 打开添加车站模态框
        openAddModal: function() {
            var _this = this;
            _this.$addModal.find('[name=Name]').val('').focus()
                     .end().find('[name=Spell]').val('')
                     .end().openModal({ dismissible: false, opacity: 0.5 });
        },

        // 打开修改车站模态框
        openEditModal: function(id, name, spell) {
            var _this = this;
            _this.$editModel.find('[name=Id]').val(id)
                      .end().find('[name=Name]').val(name).focus()
                      .end().find('[name=Spell]').val(spell)
                      .end().openModal({ dismissible: false, opacity: 0.5 });
        },

        // 保存车站信息
        onSave: function($form) {
            var model = common.formJsonfiy($form),
                _this = this;

            if (_this.isModelLeagal(model)) {
                common.ajax({
                    url: '/Admin/EditStation',
                    data: {
                        json: JSON.stringify(model)
                    }
                }).done(function(res) {
                    if (res.code == 100) {
                        Materialize.toast('操作成功:(=', 1000, '', function() {
                            window.location.reload();
                        });
                    } else if (res.code == 118) {
                        Materialize.toast('操作失败，已存在相同名称的车站:(=', 3000);
                    } else {
                        Materialize.toast('操作失败，请稍后重试:(=', 3000);
                    }
                });
            }
        },

        // 模型验证
        isModelLeagal: function(model) {
            if (!model.Name) {
                Materialize.toast('车站名称不能为空:(=', 3000);
                return false;
            }

            return true;
        }
    };

})(window, jQuery);