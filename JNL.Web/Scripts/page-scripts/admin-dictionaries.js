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
                _this.itemClick($(this));
            });

            $('.collapsible li').each(function () {
                // 为编辑按钮绑定事件
                var $editBtns = $(this).find('.edit');
                $editBtns.eq(0).on('click', function (e) {
                    e.stopPropagation();

                    _this.openModal($(this).parent().parent(), true);
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

                var type = $item.data('type'),
                    parentId = $item.data('parent'),
                    level = $item.data('level'),
                    isFirstLevel = level == 0;

                _this.makeLoading($itemBody);

                // 加载子级
                _this.loadChildrenData(type, parentId, isFirstLevel).done(function (res) {

                    _this.hideLoading();

                    if (_this.hasData(res)) {
                        var $children = _this.renderChildren(res.data);
                        $itemBody.append($children);

                        // 激活materialize的工具提示
                        $('.tooltipped').tooltip({
                            delay: 50
                        });
                    } else {
                        $itemBody.append('此项没有子级了:(=');
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
            //var isBottom = data[0].HasChildren;

            //if (isBottom) {
            //    return _this.renderAsBottomLevel(data);
            //} else {
            //    return _this.renderAsNonBottomLevel(data);
            //}

            return _this.renderAsNonBottomLevel(data);

        }, // end renderChildren

        // 将给定数据渲染成非底层（还有子级）的选项列表并返回
        renderAsNonBottomLevel: function (data) {

            var _this = this;

            var $list = $('<ul class="collapsible collapsible-accordion" data-collapsible="accordion"></ul>');

            data.forEach(function (item) {
                var $li = _this.addNonBottomChild(item);

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

                var $li = _this.addBottomChild(item);

                $list.append($li);
            });

            return $list;

        },

        // 为指定项添加非底层子级项
        addNonBottomChild: function (data) {

            var _this = this;

            var $li = $('<li data-id="{0}" data-type="{1}" data-tname="{2}" data-parent="{3}" data-childtype="{4}"></li>'.format(data.Id, data.Type, data.Name, data.Id, data.childType)),
                    $header = $('<div class="collapsible-header"></div>'),
                    $body = $('<div class="collapsible-body"></div>'),
                    createdDoms = [];

            var $expandIcon = $('<i class="expand mdi-content-add" title="展开"></i>');
            var $title = $('<span class="summary-title description">{0}</span>'.format(data.Name));

            createdDoms.push($expandIcon);
            createdDoms.push($title);

            // 创建添加图标并绑定事件
            if (data.HasChildren || data.childType > 0) {
                var $addIcon = $('<i class="edit mdi-action-note-add ml100" title="添加子级"></i>');
                $addIcon.on('click', function (e) {
                    e.stopPropagation();
                    _this.openModal($li, true);
                });

                createdDoms.push($addIcon);
            }

            // 创建修改图标并绑定事件
            var $editIcon = $('<i class="edit mdi-editor-mode-edit" title="更改名称"></i>');
            $editIcon.on('click', function (e) {
                e.stopPropagation();
                _this.openModal($li, false);
            });

            createdDoms.push($editIcon);

            $header.append(createdDoms);

            $li.append([$header, $body]);
            $li.on('click', function (e) {

                e.stopPropagation();
                e.preventDefault();

                _this.itemClick($(this));
            });

            return $li;
        },

        // 为指定项添加底层子级项
        addBottomChild: function (data) {

            var _this = this;

            var $li = $('<li data-id="{0}" data-type="{1}" data-tname="{2}" class="tree-bottom tooltipped" data-position="bottom" data-delay="50" data-tooltip="点击可进行编辑"><span class="description">{5}</span></li>'.format(data.Id, data.Type, data.Name));

            $li.on('click', function (e) {
                e.preventDefault();
                e.stopPropagation();

                _this.openModal($(this), false);
            });

            return $li;
        },

        // 加载指定项的子级
        loadChildrenData: function (type, parentId, isFirstLevel) {

            var params = { parent: parentId };
            if (isFirstLevel) {
                params.type = type;
            }

            return common.ajax({
                url: '/Admin/GetDictories',
                data: params
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
        openModal: function ($item, isAdding) {

            var _this = this;
            _this.$editItem = $item;
            _this.isAdding = isAdding;

            var $editTitle = $('#editTitle'),
                $editInput = $('#editInput'),
                desc = $item.find('.description:first').text();

            if (isAdding) {
                $editTitle.text('为“{0}”添加子级'.format(desc));
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
                var id = _this.$editItem.data('id'),
                    model = { Id: id, Name: input };

                if (_this.isAdding) {
                    model.Id = 0;
                    model.ParentId = id;
                    model.Type = _this.$editItem.data('childtype');

                    common.ajax({
                        url: '/Admin/EditDictionary',
                        data: {
                            json: JSON.stringify(model)
                        }
                    }).done(function(res) {
                        if (res.code == 108) {
                            var $container = _this.$editItem.find('.collapsible-body:first .collapsible:first'),
                                childtype = $container.find('li:first').data('childtype');

                            res.data.childType = childtype || 0;

                            var $li = _this.addNonBottomChild(res.data);
                            _this.$editItem.find('.collapsible-body:first .collapsible:first').append($li);

                            // 若当前项没有展开，则展开它
                            if (!_this.$editItem.data('expanded')) {
                                _this.$editItem.click();
                            }

                            Materialize.toast('添加成功:(=', 3000);
                        }
                    });
                } else {
                    common.ajax({
                        url: '/Admin/EditDictionary',
                        data: {
                            json: JSON.stringify(model)
                        }
                    }).done(function (res) {
                        if (res.code == 108) {
                            _this.$editItem.find('.description').text(input);
                            Materialize.toast('操作成功:(=', 3000);
                        }

                        Materialize.toast('操作失败，请稍后重试:(=', 3000);
                    });
                }
            } else {
                Materialize.toast('输入为空，无法保存！', 3000);
            }

        }

    };

})(window, jQuery);


