(function (window, $) {

    $(function () {

        page.init();

        page.bindEvents();

    });

    window.page = {
        $loading: null,
        $modal: null,

        $editItem: null,
        isAdding: false,

        // 初始化
        init: function () {

            var _this = this;

            _this.$loading = $('#loading');
            _this.$modal = $('#modal');

        },

        // 绑定事件
        bindEvents: function () {

            var _this = this;

            $('.collapsible li').on('click', function () {
                page.itemClick($(this));
            });

            $('.collapsible li').each(function () {
                // 为编辑按钮绑定事件
                var $editBtns = $(this).find('.edit');
                $editBtns.eq(0).on('click', function (e) {
                    e.stopPropagation();
                    _this.openModal($(this).parent().parent(), true);
                });

                $editBtns.eq(1).on('click', function (e) {
                    e.stopPropagation();
                    _this.openModal($(this).parent().parent(), false);
                });

                $editBtns.eq(2).on('click', function (e) {
                    e.stopPropagation();

                    _this.deleteItem($(this).parent().parent());
                });
            });

            // 保存
            $('#btnSave').on('click', function () {
                _this.saveClick();
            });

        },

        // 选项点击事件
        itemClick: function ($item) {

            var _this = this;

            var loaded = $item.data('loaded'),
                $itemBody = $item.find('.collapsible-body');

            // 第一次加载子级
            if (!loaded) {

                var id = $item.data('id');

                _this.makeLoading($itemBody);

                // 加载子级
                _this.loadChildrenData(id).done(function (res) {

                    _this.hideLoading();

                    if (_this.hasData(res)) {
                        var $children = _this.renderChildren(res.data);
                        $itemBody.append($children);

                        // 激活materialize的工具提示
                        $('.tooltipped').tooltip({
                            delay: 50
                        });
                    } else {
                        Materialize.toast('此项没有子级了:(=', 3000);
                    }
                });

                // 将此项状态置为已加载过
                $item.data('loaded', true);

            }

            _this.changeExpandStatus($item);
            _this.changeIconClass($item);
            _this.toggleExpandChildren($item);

        }, // end itemClick

        // 展开或折叠指定选项的子级
        toggleExpandChildren: function ($item) {

            var _this = this,
                hasExpanded = $item.data('expanded');

            // 展开/折叠其子级
            var $itemBody = $item.find('.collapsible-body:first');
            _this.toggleExpand($itemBody, !hasExpanded);

            // 折叠其兄弟元素
            $item.siblings().each(function () {
                if ($(this).data('expanded')) {
                    var $this = $(this);
                    _this.changeExpandStatus($this);
                    _this.changeIconClass($this);
                    _this.toggleExpand($this.find('.collapsible-body:first'), true);
                }
            });

        },

        // 改变选项的展开/折叠状态
        changeExpandStatus: function ($item) {

            var hasExpanded = $item.data('expanded');
            $item.data('expanded', !hasExpanded);

        },

        // 改变指定选项的展开/折叠图标样式
        changeIconClass: function ($item) {

            var hasExpanded = $item.data('expanded'),
                $expandIcon = $item.find('.expand:first'),
                expandClass = 'mdi-content-add', // 展开状态图标样式
                collapseClass = 'mdi-content-remove'; // 折叠状态图标样式

            // 改变选项左侧图标样式
            if (hasExpanded) {
                $expandIcon.removeClass(expandClass).addClass(collapseClass);
            } else {
                $expandIcon.removeClass(collapseClass).addClass(expandClass);
            }

        },

        // 展开/折叠指定元素
        toggleExpand: function ($elem, isExpended) {

            if (isExpended) {
                $elem.stop(true, false).slideUp({ duration: 350, easing: "easeOutQuart", queue: false, complete: function () { $(this).css('height', ''); } });
            } else {
                $elem.stop(true, false).slideDown({ duration: 350, easing: "easeOutQuart", queue: false, complete: function () { $(this).css('height', ''); } });
            }

        },

        // 将加载到的数据渲染成子级列表后返回
        renderChildren: function (data) {

            var _this = this;

            // 标识当前层级是否已经到最底层（意味着不再有子级）
            var isBottom = data[0].IsBottom;

            if (isBottom) {
                return _this.renderAsBottomLevel(data);
            } else {
                return _this.renderAsNonBottomLevel(data);
            }

        }, // end renderChildren

        // 将给定数据渲染成非底层（还有子级）的选项列表并返回
        renderAsNonBottomLevel: function (data) {

            var _this = this;

            var $list = $('<ul class="collapsible collapsible-accordion" data-collapsible="accordion"></ul>');

            data.forEach(function (item) {
                var $li = $('<li data-id="{0}" data-tid="{1}" data-tname="{2}" data-sid="{3}" data-sname="4"></li>'.format(item.Id, item.TopestTypeId, item.TopestName, item.SecondLevelId, item.SecondLevelName)),
                    $header = $('<div class="collapsible-header"></div>'),
                    $body = $('<div class="collapsible-body"></div>');

                var $expandIcon = $('<i class="expand mdi-content-add" title="展开"></i>');
                var $title = $('<span class="summary-title description">{0}</span>'.format(item.Description));

                // 创建添加图标并绑定事件
                var $addIcon = $('<i class="edit mdi-action-note-add ml100" title="添加子级"></i>');
                $addIcon.on('click', function (e) {
                    e.stopPropagation();
                    _this.openModal($li, true);
                });

                // 创建修改图标并绑定事件
                var $editIcon = $('<i class="edit mdi-editor-mode-edit" title="更改名称"></i>');
                $editIcon.on('click', function (e) {
                    e.stopPropagation();
                    _this.openModal($li, false);
                });

                $header.append([$expandIcon, $title, $addIcon, $editIcon]);

                $li.append([$header, $body]);
                $li.on('click', function (e) {

                    e.stopPropagation();
                    e.preventDefault();

                    _this.itemClick($(this));
                });

                $list.append($li);

                // 使用materialize-css的折叠功能
                //$list.collapsible({ accordion: false });

            });

            return $list;

        },

        // 将给定数据渲染成底层（意味着不再有子级）的选项列表并返回
        renderAsBottomLevel: function (data) {

            var _this = this;

            var $list = $('<ul></ul>');
            data.forEach(function (item) {

                var $li = $('<li data-id="{0}" data-tid="{1}" data-tname="{2}" data-sid="{3}" data-sname="4" class="tree-bottom tooltipped" data-position="bottom" data-delay="50" data-tooltip="点击可进行编辑"><span class="description">{5}</span></li>'.format(item.Id, item.TopestTypeId, item.TopestName, item.SecondLevelId, item.SecondLevelName, item.Description));

                $li.on('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    _this.openModal($(this), false);
                });

                $list.append($li);
            });

            return $list;

        },

        // 加载指定项的子级
        loadChildrenData: function (itemId) {

            return common.ajax({
                url: '/Common/GetList',
                data: {
                    TableName: 'RiskSummary',
                    Conditions: 'ParentId=' + itemId
                }
            });

        },

        // 判断异步请求返回的json中是否包含需要的数据
        hasData: function (ajaxRes) {

            return ajaxRes.code == 108 && ajaxRes.data instanceof Array && ajaxRes.data.length > 0;

        },

        // 显示loading动画
        makeLoading: function ($target) {
            this.$loading.show().appendTo($target);
        },

        // 隐藏loading动画
        hideLoading: function () {
            this.$loading.hide().appendTo('body');
        },

        // 打开模态框
        openModal: function ($item, addOrEdit) {

            var _this = this;
            _this.$editItem = $item;
            _this.isAdding = addOrEdit;

            var $editTitle = $('#editTitle'),
                $editInput = $('#editInput'),
                desc = $item.find('.description:first').text();

            if (addOrEdit) {
                $editTitle.text('添加子级');
                $editInput.val('');
            } else {
                $editTitle.text('修 改');
                $editInput.val(desc);
            }

            _this.$modal.openModal({ dismissible: false, opacity: 0.5 });

        },

        // 保存按钮点击事件
        saveClick: function () {

            var _this = this;

            var input = $('#editInput').val();
            if (input) {
                var id = _this.$editItem.data('id');
                if (_this.isAdding) {

                    var tid = _this.$editItem.data('tid'),
                        tname = _this.$editItem.data('tname'),
                        sid = _this.$editItem.data('sid'),
                        sname = _this.$editItem.data('sname');

                    var model = {
                        ParentId: id,
                        TopestTypeId: tid,
                        TopestName: tname,
                        SecondLevelId: sid,
                        SecondLevelName: sname,
                        Description: input
                    };

                    common.ajax({
                        url: '/Admin/AddSummary',
                        data: { json: JSON.stringify(model) }
                    }).done(function(res) {

                    });

                } else {

                    common.ajax({
                        url: '/Admin/UpdateSummary',
                        data: {
                            id: id,
                            desc: input
                        }
                    }).done(function (res) {
                        if (res.code == 100) {
                            _this.$editItem.find('.description').text(input);
                        }

                        Materialize.toast(res.msg, 3000);
                    });

                }
            } else {
                Materialize.toast('添加或者修改项不能为空，无法保存！', 3000);
            }

        }

    };

})(window, jQuery);


