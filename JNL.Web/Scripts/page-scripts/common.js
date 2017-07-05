(function () {
    window.common = {
        pickdate: function(selector) {
            var _selector = selector || '.datepicker';
            $(_selector).pickadate({
                monthsFull: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                monthsShort: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                weekdaysFull: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
                weekdaysShort: ['日', '一', '二', '三', '四', '五', '六'],
                selectMonths: true,
                selectYears: 15,
                format: 'yyyy-mm-dd',

                // Buttons
                today: '今天',
                clear: '清除',
                close: '关闭'
            });
        },

        computeOffsetTop: function (dom) {
            function compute(dom) {
                if (dom.offsetParent) {
                    return dom.offsetTop + compute(dom.offsetParent);
                } else {
                    return dom.offsetTop;
                }
            }

            return compute(dom);
        },

        computeOffsetLeft: function (dom) {
            function compute(dom) {
                if (dom.offsetParent) {
                    return dom.offsetLeft + compute(dom.offsetParent);
                } else {
                    return dom.offsetLeft;
                }
            }

            return compute(dom);
        },

        submitForbidden: function (selector, text) {
            function Forbidden(selector, text) {
                this.$dom = selector;
                if (!(this.$dom instanceof jQuery)) {
                    this.$dom = $(selector);
                }

                this.originalText = this.$dom.text();
                this.newText = text;
                this.intervalId = 0;

                this.disabled();
            }

            Forbidden.prototype.disabled = function () {
                this.$dom.prop('disabled', true);

                var counter = 0, _this = this;
                this.intervalId = setInterval(function () {
                    counter++;
                    if (counter === 6) {
                        counter = 0;
                    }

                    var text = _this.newText;
                    for (var i = 0; i < counter; i++) {
                        text += '.';
                    }

                    _this.$dom.text(text);
                }, 500);
            };

            Forbidden.prototype.enabled = function () {
                this.$dom.prop('disabled', false).text(this.originalText);

                window.clearInterval(this.intervalId);
            }

            return new Forbidden(selector, text);
        },

        /**
         * 为页面上的选择框加载字典集
         * @param {Object[]} types 如：{ type: 1, selector: '#select', value: 'Id' }
         */
        loadDictionaries: function (targets) {
            var targetList = targets || [],
                _this = this;

            function buildParams(type) {
                return {
                    TableName: 'Dictionaries',
                    Conditions: 'Type=' + type
                };
            }

            targetList.forEach(function (target) {
                var formData = buildParams(target.type);
                _this.ajax({
                    url: '/Common/GetList',
                    data: formData
                }).done(function (res) {
                    if (res.code == 108) {
                        var $select = $(target.selector);

                        var data = res.data || [],
                            html = '';
                        data.forEach(function (model) {
                            var value = model[target.value],
                                text = model.Name,
                                selected = value == target.selected ? 'selected="selected"' : '';

                            html += '<option value="{0}" {1}>{2}</option>'.format(value, selected, text);
                        });

                        $select.append(html);
                        $select.material_select();
                    }
                });
            });
        },

        /**
         * 封装带有提示信息的方块，且可对此方块进行简单的配置
         * @param {Object} options 对方块的配置信息
         * @returns {void} 
         * @author FrancisTan
         * @since 2016-07-14
         */
        tiles: function (options) {
            var opts = $.extend({}, {
                closable: true, // 右上角会出现关闭按钮
                big: false, // 字体间距会变大
                target: '', // 生成的方块将会被append到此元素后面
                style: 'dangerous', // 方块样式，目前只有三种（success、warning、dangerous）
                text: '' // 方块内的提示信息文本
            }, options);

            var tile = $('<div class="alert"></div>');
            if (opts.closable) {
                tile.addClass('alert-dismissable');
                tile.append('<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>');
            }
            if (opts.big) {
                tile.addClass('alert-big');
            }

            var styleClass = 'alert-cyan'; // 蓝绿色
            switch (opts.style) {
                case 'success':
                    styleClass = 'alert-success';
                    break;
                case 'warning':
                    styleClass = 'alert-warning';
                    break;
                case 'dangerous':
                    styleClass = 'alert-lightred';
                    break;
            }
            tile.addClass(styleClass);
            tile.append('<p>' + opts.text + '</p>');

            tile.insertAfter(opts.target);
        },

        /**
         * 绑定页面上表单输入框的enter键按下事件
         * @param {String} inputSelector 指定一组输入表单元素做为enter事件源
         * @param {String} target 元素选择器，表示待绑定事件的dom元素
         * @returns {void} 
         * @author FrancisTan
         * @since 2016-07-14
         */
        bindEnterEvent: function (inputSelector, target) {
            $(inputSelector).on('keydown', function (e) {
                if (e.keyCode === 13) {
                    $(target).click();
                }
            });
        },

        /**
         * 对本站所有的异步请求作统一控制，所有需要异步请求的地方都必须调用此方法
         * @author FrancisTan
         * @since 2016-07-15
         */
        ajax: function (options) {
            var _this = this;
            var opts = $.extend({}, {
                url: '',
                data: { },
                type: 'POST',
                dataType: 'JSON'
            }, options);

            opts.data.async = true;

            opts.success = function (data) {
                if (data.code == 104) {
                    _this.showModal({
                        content: data.msg,
                        showCancel: false,
                        confirmText: '去登录',
                        confirm: function () {
                            window.location.href = data.backUrl;
                        },
                        cancel: function () {
                            window.location.href = data.backUrl;
                        }
                    });
                }

                if ($.isFunction(options.success)) {
                    options.success(data);
                }
            };

            opts.error = function (XMLHttpRequest, textStatus, errorThrown) {
                common.hideLoading();
                if ($.isFunction(options.error)) {
                    options.error(XMLHttpRequest, textStatus, errorThrown);
                }
            };

            return $.ajax(opts);
        },

        /**
         * 获取地址栏指定名称的参数值
         * @param {String} name 参数名称
         * @returns {String} 获取到的值或者''
         * @author FrancisTan
         * @since 2016-07-15 
         */
        getQueryParam: function (name) {
            var reg = new RegExp("(^|&){0}=([^&]*)(&|$)".format(name), "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return decodeURIComponent(r[2]);
            return '';
        },

        /**
         * 使用bootstrap模态框模拟alert
         * @returns {String} 要提示的文字消息
         * @author FrancisTan
         * @since 2016-07-15
         */
        alert: function (msg, confirm, title, cancel) {
            this.showModal({
                content: msg,
                confirm: confirm,
                cancel: cancel,
                title: title,
                showCancel: false
            });

            return this;
        },

        /**
         * 使用bootstrap模态框模拟confirm对话框
         * @param {String} msg 提示的文字消息
         * @param {Function} confirm 点击确定时执行的回调函数
         * @param {Function} cancel 点击取消时执行的回调函数
         * @returns {Object} this 
         * @author FrancisTan
         * @since 2016-07-15
         */
        confirm: function (msg, confirm, cancel) {
            this.showModal({
                content: msg,
                confirm: confirm,
                cancel: cancel
            });

            return this;
        },

        /**
         * 当页面中没有模态框时，创建一个模态框
         * @returns {void}
         * @author FrancisTan
         * @since 2016-07-15
         */
        createModal: function () {
            if ($('#myModal').length === 0) {
                $('body').append('<div class="modal splash fade splash-2 splash-ef-4 in" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="false"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><button type="button" class="close ___modal_close___" data-dismiss="modal">×</button><h3 class="___modal_title___ modal-title custom-font">Settings</h3></div><div class="modal-body"><p class="___modal_content___">Here settings can be configured...</p></div><div class="modal-footer"><a href="#" class="btn btn-default ___modal_close___" data-dismiss="modal">Close</a><a href="#" class="btn btn-success ___modal_save___" data-dismiss="modal">Save changes</a></div></div></div></div>');
            }
        },

        /**
         * 显示模态框（可基于此方法配置各种提示框）
         * 若content为jQuery对象，则在模态框消失前，函数会自动将jQuery对象恢复到原来的父节点下
         * @param {Object} opts 用于初始化模态框的数据 
         * @returns {Object} this 
         * @author FrancisTan
         * @since 2016-07-15
         */
        showModal: function (options) {
            var opts = $.extend({}, {
                title: '提示',
                content: '',
                showConfirm: true,
                showCancel: true,
                confirmText: '确 定',
                cancelText: '取 消',
                confirm: null, // 点击确定之后触发的回调函数，若此函数返回值===false，则模态框不会关闭
                cancel: null, // 点击取消之后触发的回调函数，若此函数返回值===false，则模态框不会关闭
                autoDismiss: false, // 该值指示模态框是否会自动消失
                dismissTime: 2000 // 该值指示模态框将在1500ms后自动关闭
            }, options);

            this.createModal();
            var unbindClick = function () {
                $('.___modal_save___').unbind('click.modalSave');
                $('.___modal_close___').unbind('click.modalClose');
                $('#myModal').unbind('click.modalDismiss');
                $('#myModal').children().unbind('click.modalStop');
            };

            var $originalParentNode;
            // 如果content是对dom封装的jQuery对象，则在隐藏模态框之前使之回到原来的父节点下，以免丢失此dom
            if (opts.content instanceof jQuery) {
                $originalParentNode = opts.content.parent();
            }
            var recoverNode = function () {
                if ($originalParentNode) {
                    opts.content.appendTo($originalParentNode);
                }
            };

            var execConfirm = function () {
                var callbackRes;
                if (typeof (opts.confirm) === 'function') {
                    callbackRes = opts.confirm();
                }

                if (callbackRes === false) {
                    return false;
                }
                unbindClick();
                recoverNode();
            };
            var execCancel = function () {
                var callbackRes;
                if (typeof (opts.cancel) === 'function') {
                    callbackRes = opts.cancel();
                }

                if (callbackRes === false) {
                    return false;
                }
                unbindClick();
                recoverNode();
            };

            $('.___modal_title___').html(opts.title);
            $('.___modal_content___').empty().append(opts.content);
            $('.___modal_close___.btn').html(opts.cancelText);
            $('.___modal_save___').html(opts.confirmText);

            if (!opts.showConfirm) {
                $('.___modal_save___.btn').hide();
            } else {
                $('.___modal_save___.btn').show();
            }

            if (!opts.showCancel) {
                $('.___modal_close___.btn').hide();
            } else {
                $('.___modal_close___.btn').show();
            }

            $('.___modal_save___').bind('click.modalSave', execConfirm);
            $('.___modal_close___').bind('click.modalClose', execCancel);
            $('#myModal').bind('click.modalDismiss', execCancel);
            $('#myModal').children().bind('click.modalStop', function () {
                return false;
            });


            $('#myModal').modal('show');

            if (opts.autoDismiss) {
                setTimeout(function () {
                    $('.___modal_close___').click();
                }, opts.dismissTime);
            }

            return this;
        },

        hideModal: function () {
            $('.___modal_close___').click();
        },

        /**
         * 展示一个会自动消息的提示框
         * @param {String} msg 待展示的消息 
         * @param {Number} time 指定提示框会在规定时间（ms）消失
         * @returns {} 
         * @since 2016-07-15
         */
        tips: function (msg, time, callback) {
            this.showModal({
                showCancel: false,
                content: msg,
                autoDismiss: true,
                dismissTime: time
            });

            return this;
        },

        /**
         * 使用spin.js插件创建一个loading遮罩层
         * @param {Object} option loading层的配置，参考地址：http://www.html5tricks.com/demo/jquery-loading-spin-js/index.html#?lines=13&length=15&width=4&radius=10&corners=0&rotate=77&trail=81&speed=1.0&shadow=on&hwaccel=on
         * @returns {Object} this 
         * @since 2016-07-22
         */
        showLoading: function (option) {
            var opts = $.extend({}, {
                lines: 13, // The number of lines to draw
                length: 15, // The length of each line
                width: 5, // The line thickness
                radius: 10, // The radius of the inner circle
                corners: 0, // Corner roundness (0..1)
                rotate: 77, // The rotation offset
                color: '#000', // #rgb or #rrggbb
                speed: 1, // Rounds per second
                trail: 81, // Afterglow percentage
                shadow: false, // Whether to render a shadow
                hwaccel: true, // Whether to use hardware acceleration
                className: 'spinner', // The CSS class to assign to the spinner
                zIndex: 99999999, // The z-index (defaults to 2000000000)
                back: 'rgba(0,0,0,0.1)', // background -- added by myself
                showBack: true, // show back or not -- added by myself
                // top: 'auto', // Top position relative to parent in px
                // left: 'auto' // Left position relative to parent in px
            }, option);

            if (!this.spinner) {
                this.spinner = new Spinner(opts);
            }

            var targetNode;
            if (opts.showBack) {
                var $target = $('#___loading_shadow___');
                if ($target.length === 0) {
                    $target = $('<div id="___loading_shadow___" style="position: fixed; left:0; top:0; right:0; bottom:0; z-index: 99999999; background:{0};"></div>'.format(opts.back));
                    $target.appendTo(document.body);
                }
                targetNode = $target.show()[0];
            } else {
                targetNode = document.body;
            }

            this.spinner.spin(targetNode);

            return this;
        },

        /**
         * 隐藏loading遮罩层
         * @returns {Object} this 
         */
        hideLoading: function () {
            if (this.spinner) {
                this.spinner.spin();
            }
            $('#___loading_shadow___').hide();

            return this;
        },

        /**
         * 处理由服务器端返回的类似 \/Date(1463481481400)\/ 格式的日期，并返回js的Date对象
         * @param {String} dateStr 待处理的日期字符串
         * @returns {Date} 
         */
        processDate: function (dateStr) {
            try {
                var time = parseInt(/Date\((\d+)\)/.exec(dateStr)[1]);
                return new Date(time);
            } catch (e) {
                return '';
            }
        },

        /**
         * 尝试将指定的数据转换为json对象，若失败则返回原值
         */
        tryParseJson: function (data) {
            try {
                return $.parseJSON(data);
            } catch (e) {
                return data;
            }
        },

        /**
         * 将form表单需要提交的数据保存到一个json对象中返回
         * @param {String} selector 元素选择器
         * @returns {Object} 
         */
        formJsonfiy: function (selector) {
            var $form;
            if (selector instanceof jQuery) {
                $form = selector;
            } else {
                $form = $(selector);
            }

            var data = $form.serializeArray();
            var json = {};

            for (var i = 0; i < data.length; i++) {
                json[data[i].name] = data[i].value;
            }

            return json;
        },

        /**
         * 将给定的json对象的值填充到form表单中对应的元素中（name与json中的键对应）
         * @param {String} form元素选择器
         * @param {Object} json 
         * @returns {void} 
         */
        fillForm: function (form, json) {
            var _this = this;
            var $form = $(form);
            for (var key in json) {
                if (json.hasOwnProperty(key)) {
                    var value = json[key];
                    if (/Date\(\d+\)/.test(value)) {
                        value = _this.processDate(value).format('yyyy-MM-dd HH:mm:ss');
                    } else if (/True|False/.test(value)) {
                        value = value == 'True' ? true : false;
                    }

                    var $dom = $form.find('*[name={0}]'.format(key));
                    if (_this.isFormElement($dom[0])) {
                        $dom.val(value);;
                    } else {
                        $dom.html(value);
                    }
                }
            }
        },

        /**
         * 判断给定元素是否是表单元素
         * @param {Dom} dom 
         * @returns {Boolean} 
         */
        isFormElement: function (dom) {
            try {
                var nodeName = dom.nodeName;

                return nodeName === 'INPUT' || nodeName === 'TEXTAREA' || nodeName === 'SELECT';
            } catch (e) {
                console.info(e);
                return false;
            }
        },

        /**
         * 从身份证号码中提取出出生日期
         * @param {String} identity 合法的中国大陆身份证号码
         * @returns {String} 类似于 1990-06-10 这样的日期字符串或者''
         */
        matchBirthDate: function (identity) {
            var value = identity;
            var matches = /(^[1-9]\d{5}(\d{2}(0\d|1[0-2])([012]\d|3[0-1]))\d{3}$)|(^[1-9]\d{5}([1-9]\d{3}(0\d|1[0-2])([012]\d|3[0-1]))\d{3}[0-9]|X$)/.exec(value);

            if (matches != null) {
                var dates = matches.where(function (item) {
                    return item && (item.length === 6 || item.length === 8);
                });

                if (dates.length === 1) {
                    var date = dates[0];
                    if (date.length === 6) {
                        date = '19' + date;
                    }

                    date = date.replace(/(\d{4})(\d{2})(\d{2})/, '$1-$2-$3');
                    return date;
                }
            }

            return '';
        },

        /**
         * 使用指定数据填充select元素
         * @param {String} selector select元素选择器
         * @param {Object} options 配置对象，其中包括：
         *      data[数据集合]
         *      text[将作为option的text的字段名称]
         *      value[将作为option的value的字段名称] 
         *      selectedFunc[指定默认选中项的判断方法，该方法返回true/false]
         */
        buildOptions: function (selector, options) {
            var _this = this;
            if (options.data instanceof Array) {
                var data = options.data;
                var selectedFunc = typeof (options.selectedFunc) === 'function'
                    ? options.selectedFunc
                    : function () { return false };

                for (var i = 0; i < data.length; i++) {
                    var value = data[i][options.value];
                    var text = data[i][options.text];
                    var selected = selectedFunc(data[i]) ? 'selected' : '';

                    $(selector).append('<option value="{0}" {1}>{2}</option>'.format(value, selected, text));
                }
            }
        },

        /**
         * 验证给定值是否是有效的时间
         * @param {any} value 待验证的字符串、数字或者其他类型
         * @returns {Boolean}
         */
        isValidDate: function (value) {
            try {
                var date = new Date(value);
                if (date == 'Invalid Date') {
                    return false;
                }

                return true;
            } catch (e) {
                console.error(e);

                return false;
            }
        },

        /**
         * 使用jquery.datatable插件创建DataTable对象
         * @param {String} tableSelector 表格元素选择器
         * @param {Object} options 表格的配置信息
         * @returns {Object} 创建好的DataTable对象 
         */
        dataTable: function (tableSelector, options) {
            var opts = $.extend({}, {
                sPaginationType: 'bootstrap', //分页风格，自定义分页风格 它定义在/Content/chrisma-master-content/js/charisma.js的底部
                sAjaxSource: '', //给服务器发请求的url
                sServerMethod: 'POST', // 指定发送请求的方法（插件默认是GET）
                bServerSide: true, //开启服务器模式，使用服务器端处理配置datatable。注意：sAjaxSource参数也必须被给予为了给datatable源代码来获取所需的数据对于每个画。 这个翻译有点别扭。开启此模式后，你对datatables的每个操作 每页显示多少条记录、下一页、上一页、排序（表头）、搜索，这些都会传给服务器相应的值。 
                fnServerParams: function (aoData) { // 向服务器发送额外的参数

                },
                //sDom: "<'row'<'col-md-6'l><'col-md-6'f>r>t<'row'<'col-md-12'i><'col-md-12 center-block'p>>", //自定义布局
                iDisplayLength: 10, //每页显示10条数据
                bAutoWidth: true, //宽度是否自动，感觉不好使的时候关掉试试
                bLengthChange: true, //开关，是否显示一个每页长度的选择条（需要分页器支持）
                bFilter: false,

                oLanguage: {
                    //下面是一些汉语翻译
                    "sSearch": "搜索",
                    "sLengthMenu": "每页显示 _MENU_ 条记录",
                    "sZeroRecords": "没有检索到数据",
                    "sInfo": "显示 _START_ 至 _END_ 条 &nbsp;&nbsp;共 _TOTAL_ 条",
                    "sInfoFiltered": "(筛选自 _MAX_ 条数据)",
                    "sInfoEmtpy": "没有数据",
                    "sProcessing": "正在加载数据...",
                    "oPaginate":
                    {
                        "sFirst": "首页",
                        "sPrevious": "前一页",
                        "sNext": "后一页",
                        "sLast": "末页"
                    }
                },
                bProcessing: true, //开启读取服务器数据时显示正在加载中……特别是大数据量的时候，开启此功能比较好
                aoColumns: [
                    //必填属性，这个属性下的设置会应用到所有列，按顺序没有是空
                    //{ "mData": 'nickname' }, //mData 表示发请求时候本列的列明，返回的数据中相同下标名字的数据会填充到这一列
                    //{ "mData": 'follower_count' },
                    //{ "sDefaultContent": '' }, // sDefaultContent 如果这一列不需要填充数据用这个属性，值可以不写，起占位作用
                    //{ "sDefaultContent": '', "sClass": "action" },//sClass 表示给本列加class
                ],
                fields: [], // 自定义属性，对aoColumns的封装
                aoColumnDefs: [
    //和aoColums类似，但他可以给指定列附近爱属性
                    //{ "bSortable": false, "aTargets": [1, 3, 6, 7, 8, 9] },  //这句话意思是第1,3,6,7,8,9列（从0开始算） 不能排序
                    //{ "bSearchable": false, "aTargets": [1, 2, 3, 4, 5, 6, 7, 8, 9] }, //bSearchable 这个属性表示是否可以全局搜索，其实在服务器端分页中是没用的
                ],
                //aaSorting: [[1, "desc"]], //默认排序
                fnRowCallback: function (nRow, aData, iDisplayIndex) { // 当创建了行，但还未绘制到屏幕上的时候调用，通常用于改变行的class风格 
                    //if (aData.status == 1) {
                    //    $('td:eq(8)', nRow).html("<span class='text-error'>审核中</span>");
                    //} else if (aData.status == 4) {
                    //    $('td:eq(8)', nRow).html("<span class='text-error'>审核失败</span>");
                    //} else if (aData.active == 0) {
                    //    $('td:eq(8)', nRow).html("<span>隐藏</span>");
                    //} else {
                    //    $('td:eq(8)', nRow).html("<span class='text-success'>显示</span>");
                    //}
                    //$('td:eq(9)', nRow).html("<a href='' user_id='" + aData.user_id + "' class='ace_detail'>详情</a>");
                    //if (aData.status != 1 && aData.status != 4 && aData.active == 0) {
                    //    $("<a class='change_ace_status'>显示</a>").appendTo($('td:eq(9)', nRow));
                    //} else if (aData.status != 1 && aData.status != 4 && aData.active == 1) {
                    //    $("<a class='change_ace_status'>隐藏</a>").appendTo($('td:eq(9)', nRow));
                    //}
                    //return nRow;
                },
                fnInitComplete: function (oSettings, json) { //表格初始化完成后调用 在这里和服务器分页没关系可以忽略

                }
            }, options);

            if (typeof (options.fnServerParams) === 'function') {
                opts.fnServerParams = function (aoData) {
                    aoData.push({
                        // 遵从系统规范，对所有异步请求添加async参数，值为true
                        name: 'async',
                        value: 'true'
                    });

                    options.fnServerParams(aoData);
                };
            }

            if (opts.aoColumns.length === 0 && opts.fields.length > 0) {
                for (var i = 0; i < opts.fields.length; i++) {
                    opts.aoColumns.push({ 'mDataProp': opts.fields[i] });
                }
            }

            return $(tableSelector).dataTable(opts);
        },

        /**
         * 使用插件 jquery.uploadify 创建文件上传实例
         * @param {String} fileInputId input:file元素的id
         * @param {Object} options 插件配置信息对象
         * @author FrancisTan
         * @since 2016-05-13 12:12
         * @returns {Object} Uploadify对象 
         */
        uploadify: function (fileInputId, options) {
            var opts = $.extend({}, {
                swf: '/Scripts/plugins/jquery.uploadify/uploadify.swf', // 必填，插件所依赖的swf文件的路径，没有此文件插件将无法工作
                uploader: '', // 必填，定义服务器端上传数据处理程序的路径
                uploadLimit: 999, // 定义允许的最大上传数量
                auto: true, // 设置auto为true，当文件被添加至上传队列时，将会自动上传，当设置为false时，可通过方法upload手动上传
                buttonClass: '', // 必须，为上传按钮添加类名，默认值为''
                buttonCursor: 'hand', // 鼠标经过上传按钮时，鼠标的形状。可选值为‘hand’(手形) 和 ‘arrow’(箭头)
                buttonImage: null, // 定义“浏览”按钮背景图像的路径。给按钮设置背景图像的代码最好写在CSS文件中。参照 http://www.uploadify.com/documentation/uploadify/buttonimage/
                buttonText: '选择文件', // 定义显示在默认按钮上的文本
                checkExisting: '', // 定义检查目标文件夹中是否存在同名文件的服务器处理程序路径（服务器处理程序应该返回 1/0 表示是否存在同名文件已被上传）
                debug: false, // 当其值为true时，开启SWFUpload调试模式，此时上传文件的日志信息会在浏览CONSOLE中打印出来
                fileObjName: 'uploadifyFile', // 定义上传数据处理文件中接收数据使用的文件对象名
                fileSizeLimit: '30MB', // 上传文件大小限制，支持B、KB、MB以及GB等单位
                fileTypeDesc: 'All Files', // 可选择的文件类型的描述。此字符串出现在浏览文件对话框的文件类型下拉菜单中
                fileTypeExts: '*.*', // 定义允许上传的文件后缀，例：'*.jpg; *.png;' 
                formData: {}, // 定义在文件上传时需要一同提交的其他数据对象，若想动态添加数据，可在事件onUploadStart中添加如下代码 $("#file_upload").uploadify("settings", "formData", {})，将formData修改;
                height: 30, // 上传按钮的高度（单位：像素），插件默认值30
                width: 120, // 定义浏览按钮的宽度
                itemTemplate: '', // itemTemplate选项允许你为每一个添加到队列中选项设定一个不同HTML模板，使用方法参照 http://www.uploadify.com/documentation/uploadify/itemtemplate/
                method: 'post', // 上传文件的提交方法，取值 post 或 get
                multi: true, // 是否允许上传多个文件，设置值为false时，一次只能选中一个文件
                overrideEvents: [], // 该项定义了一组默认脚本中你不想执行的事件名称，如：['onUploadProgress']
                preventCaching: true, // 如果设置为真，一个随机的值添加到SWF文件的URL，因此它不会缓存
                progressData: 'percentage', // 设置显示在上传进度条中的数据类型，可选项时百分比（percentage）或速度（speed）
                queueID: '', // queueID选项允许你设置一个拥有唯一ID的DOM元素来作为显示上传队列的容器
                queueSizeLimit: 999, // 上传队列中一次可容纳的最大条数，若选择的文件数量超过这个值，则onSelectError事件将被触发
                removeCompleted: true, // 不设置该选项或者将其设置为false，将使上传队列中的项目始终显示于队列中，直到点击了关闭按钮或者队列被清空
                removeTimeout: 1.5, // 设置上传完成后从上传队列中移除的时间（单位：秒）
                requeueErrors: false, // 设置为真时，上传队列重置或上传多次重试时，返回错误信息
                successTimeout: 30, // 表示uploadify的成功等待时间（单位：秒，默认30秒）

                // 以下为事件
                onCancel: null, // 设置onCancel选项，在文件上传被取消时，将允许运行一个自定义函数，插件将传递给此方法的参数为:file（将被取消上传的文件对象）
                onClearQueue: null, // 设置onClearQueue选项，上传队列清空（激活ancel方法）时，将允许运行一个自定义函数，参数：queueItemCount（被取消上传的文件个数）
                onDestroy: null, // 销毁Uploadify实例（调用destroy方法）时触发该事件，无参数
                onDialogClose: null, // 当浏览文件对话框关闭时触发该事件。如果该事件被添加到overrideEvents属性中，在添加文件到队列中发生错误时，将不会弹出默认错误信息，参数参照 http://www.uploadify.com/documentation/uploadify/ondialogclose/
                onDialogOpen: null, // 在浏览文件对话框被打开"前一瞬"触发该事件
                onDisable: null, // 调用disable方法禁用Uploadify实例时触发该事件
                onEnable: null, // 调用disable方法启用Uploadify实例时触发该事件
                onFallback: null, // 浏览器检测不到兼容版本的Flash时触发该事件
                onInit: null, // 调用Uploadify初始化结束时触发该事件
                onQueueComplete: null, // 队列中的所有文件被处理完成时触发该事件，参数参照 http://www.uploadify.com/documentation/uploadify/onqueuecomplete/
                onSelect: null, // 每添加一个文件至上传队列时触发该事件
                onSelectError: null, // 选择文件返回错误时触发该事件。每一个文件返回错误都会触发该事件，参数参照 http://www.uploadify.com/documentation/uploadify/onselecterror/
                onSWFReady: null, // 当flash按钮载入完毕时触发该事件
                onUploadComplete: null, // 每一个文件上传完成都会触发该事件，不管是上传成功还是上传失败，建议使用 onUploadSuccess event or onUploadError event 来处理单个文件上传完成的逻辑
                onUploadError: null, // 单个文件上传失败时触发该事件
                onUploadProgress: null, // 上传进度更新时触发该事件，参数参照 http://www.uploadify.com/documentation/uploadify/onuploadprogress/
                onUploadStart: null, // 在开始上传之前的瞬间会触发该事件，参数file
                onUploadSuccess: null, // 单个文件上传成功时触发该事件，参数参照 http://www.uploadify.com/documentation/uploadify/onuploadsuccess/

                // 以下为方法，直接通过$(selector).uplodify(methodname)调用
                // cancel  取消上传对象
                // destroy 销毁Uploadify实例，并返回原文件域
                // disable 控制选择文件按钮是否可用，与form表单一样，使用true/false控制
                // settings 返回或更新一个Uploadify实例的值，参数参照 http://www.uploadify.com/documentation/uploadify/settings/
                // stop 停止当前上传
                // upload 上传指定文件或队列中的所有文件
            }, options);

            return $(fileInputId).uploadify(opts);
        },

        /**
         * 创建富文本编辑器，依赖插件wangEditor
         * @param {String} editorId 不带#的编辑器element id
         * @param {Object} options 用于初始化编辑器的配置信息
         * @returns {wangEditor} 
         */
        createWangEditor: function (editorId, options) {
            var opts = $.extend({}, {
                html: '',
                menus: ['source', '|', 'bold', 'underline', 'italic', 'strikethrough', 'eraser', 'forecolor', 'bgcolor', '|', 'quote', 'fontfamily', 'fontsize', 'head', 'unorderlist', 'orderlist', 'alignleft', 'aligncenter', 'alignright', '|', 'link', 'unlink', 'table', 'emotion', '|', 'img', 'video', 'location', 'insertcode', '|', 'undo', 'redo', 'fullscreen'], // | 是菜单分割线
                zindex: 10000, // 编辑器全屏时的z-index
                printLog: true,
                jsFilter: true,
                pasteFilter: true,
                uploadImgUrl: '/Common/FileUpload?fileType=2', // 图片上传的地址，async参数是为了遵从系统规范(所有异步请求都必须带上此参数)
                uploadSuccess: function (data, xhr) {
                    try {
                        data = $.parseJSON(data);
                        if (data.code == 110) {
                            Materialize.toast('图片上传失败，请稍后重试', 3000);
                        } else {
                            editor.command(null, 'insertHtml', '<img src="{0}" style="max-width:100%" />'.format(data.FileRelativePath));
                        }
                    } catch (e) {
                        console.info(e);
                    } 
                }, // 图片上传成功的回调函数
                uploadTimeout: function () {
                    app.alert('连接超时，请检查您的网络');
                }, // 图片上传超时的回调函数
                uploadError: function () {
                    app.alert('上传失败，请确定您选择图片是否符合要求或尝试重新上传');
                } // 图片上传出错的回调函数
            }, options);

            wangEditor.config.printLog = opts.printLog;
            var editor = new wangEditor(editorId);

            editor.config.menus = opts.menus;
            editor.config.zindex = opts.zindex;
            editor.config.jsFilter = opts.jsFilter;
            editor.config.uploadImgUrl = opts.uploadImgUrl;
            editor.config.pasteFilter = opts.pasteFilter;
            if (typeof (opts.uploadSuccess) === 'function') {
                editor.config.uploadImgFns.onload = opts.uploadSuccess;
            }
            if (typeof (opts.uploadTimeout) === 'function') {
                editor.config.uploadImgFns.ontimeout = opts.uploadTimeout;
            }
            if (typeof (opts.uploadError) === 'function') {
                editor.config.uploadImgFns.onerror = opts.uploadError;
            }

            editor.create();

            editor.$txt.html(opts.html);

            return editor;
        },
    };

    /**
     * 找出数组中最大的元素，若数组中是json对象则先根据指定方法筛选出其中一列再寻找最大值
     * @param {} selector 
     * @returns {} 
     */
    Array.prototype.max = function (selector) {
        var array = this;
        if (typeof (selector) === 'function') {
            array = [];
            for (var i = 0; i < this.length; i++) {
                array.push(selector(this[i]));
            }
        }

        var maxValue = array[0];
        for (var j = 0; j < array.length; j++) {
            if (array[j] > maxValue) {
                maxValue = array[j];
            }
        }

        return maxValue;
    };

    /**
     * 对数组中对象的指定属性值求和
     * @param {Function} selector 指定列的方法，若不传此参数，则直接将数组元素求和
     * @returns {Number} 
     */
    Array.prototype.sum = function (selector) {
        var _this = this;
        var select = $.isFunction(selector) ? selector : function () { return _this; }

        var sum = 0;
        this.forEach(function(item) {
            sum += +select(item);
        });

        return sum;
    };

    /**
     * 判断数组中是包包含指定元素
     * @param {} element 
     * @returns {} 
     */
    Array.prototype.contains = function (element) {
        var self = this;
        for (var i = 0; i < self.length; i++) {
            if (typeof (element) === 'function') {
                if (element(self[i])) {
                    return true;
                }
            } else {
                if (self[i] == element) {
                    return true;
                }
            }
        }
        return false;
    }
    /**
     * 移除数组中按指定匹配方法找到的值
     * @param {*|Function} value 当此参数为一个方法时，第二个参数将不起作用
     * @param {Function} compareFunc 指定匹配方法function(value1, value2), 返回布尔值作为比较结果
     * @returns {void} 
     */
    Array.prototype.remove = function (value, compareFunc) {
        if (typeof (value) === 'function') {
            for (var j = 0; j < this.length; j++) {
                if (value(this[j])) {
                    this.splice(j, 1);

                    j -= 1;
                }
            }
        } else if (typeof (compareFunc) === 'function') {
            for (var i = 0; i < this.length; i++) {
                if (compareFunc(value, this[i])) {
                    this.splice(i, 1);

                    i -= 1;
                }
            }
        } else {
            for (var k = 0; k < this.length; k++) {
                if (value == this[k]) {
                    this.splice(k, 1);

                    k -= 1;
                }
            }
        }
    };
    /**
     * 对数据元素按照指定的比较方法进行冒泡排序 * 
     * @param {Function} compare 指定排序过程中两个元素之间的比较方法，调用此方法时传递参数顺序是：a.数组靠前面的元素；b.数据靠后面的元素
     * @returns {void} 
     */
    Array.prototype.bubbleSort = function (compare) {
        for (var i = 0; i < this.length - 1; i++) {
            for (var j = i + 1; j < this.length; j++) {
                if (compare(this[i], this[j])) {
                    var temp = this[i];
                    this[i] = this[j];
                    this[j] = temp;
                }
            }
        }
    };
    /**
     * 按照指定的方法选择数组中对象的部分属性，并组成新数组返回
     * @param {Function} getColumn 指示获取对象中部分属性的方法，此方法的返回值将作为新数组中的元素
     * @returns {Array} 按照指定方法筛选出来的新数组 
     */
    Array.prototype.select = function (getProperty) {
        var newArr = [];
        for (var i = 0; i < this.length; i++) {
            newArr.push(getProperty(this[i]));
        }
        return newArr;
    };
    /**
     * 根据条件筛选数据组的元素
     * @param {Function} condition 对数据的筛选条件（返回true/false）
     * @returns {Array} 本数组的一个字集 
     */
    Array.prototype.where = function (condition) {
        var subArr = [];
        for (var i = 0; i < this.length; i++) {
            if (condition(this[i])) {
                subArr.push(this[i]);
            }
        }

        return subArr;
    };

    /**
     * 根据条件返回数据中的匹配到的第一个元素
     * 若没有找到返回null
     * @param {Function} conditionFunc 匹配元素的方法
     * @returns {Object|null} 
     */
    Array.prototype.single = function (conditionFunc) {
        if (typeof (conditionFunc) !== 'function') {
            return null;
        }

        var result = null;
        this.forEach(function (value, index) {
            if (conditionFunc(value)) {
                result = value;
                return false;
            }
        });

        return result;
    };

    /**
     * 返回数组中的第一个元素
     */
    Array.prototype.first = function () {
        return this[0];
    };

    /**
     * 返回数组中最后一个元素
     */
    Array.prototype.last = function () {
        return this[this.length - 1];
    };

    /**
     * 模拟C#中的string.format方法
     * @author FrancisTan
     * @since 2016-07-15
     */
    String.prototype.format = function (args) {
        if (arguments.length > 0) {
            var result = this;
            if (arguments.length == 1 && typeof (args) == "object") {
                for (var key in args) {
                    result = result.replace(new RegExp("({" + key + "})", "g"), args[key]);
                }
            }
            else {
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i] == undefined) {
                        return "";
                    }
                    else {
                        result = result.replace(new RegExp("({[" + i + "]})", "g"), arguments[i]);
                    }
                }
            }
            return result;
        }
        else {
            return this;
        }
    }

    //日期格式化
    Date.prototype.format = function (format) {
        var o = {
            "M+": this.getMonth() + 1, //month 
            "d+": this.getDate(), //day 
            "H+": this.getHours(), //hour 
            "h+": this.getHours(), //hour 
            "m+": this.getMinutes(), //minute 
            "s+": this.getSeconds(), //second 
            "q+": Math.floor((this.getMonth() + 3) / 3), //quarter 
            "S": this.getMilliseconds() //millisecond 
        }

        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        }

        for (var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return format;
    }

    try {
        //additional functions for data table
        $.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
            return {
                "iStart": oSettings._iDisplayStart,
                "iEnd": oSettings.fnDisplayEnd(),
                "iLength": oSettings._iDisplayLength,
                "iTotal": oSettings.fnRecordsTotal(),
                "iFilteredTotal": oSettings.fnRecordsDisplay(),
                "iPage": Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
                "iTotalPages": Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
            };
        }
        $.extend($.fn.dataTableExt.oPagination, {
            "bootstrap": {
                "fnInit": function (oSettings, nPaging, fnDraw) {
                    var oLang = oSettings.oLanguage.oPaginate;
                    var fnClickHandler = function (e) {
                        e.preventDefault();
                        if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
                            fnDraw(oSettings);
                        }
                    };

                    $(nPaging).addClass('pagination').append(
                        '<ul class="pagination">' +
                            '<li class="prev disabled"><a href="#">&larr; ' + oLang.sPrevious + '</a></li>' +
                            '<li class="next disabled"><a href="#">' + oLang.sNext + ' &rarr; </a></li>' +
                            '</ul>'
                    );
                    var els = $('a', nPaging);
                    $(els[0]).bind('click.DT', { action: "previous" }, fnClickHandler);
                    $(els[1]).bind('click.DT', { action: "next" }, fnClickHandler);
                },

                "fnUpdate": function (oSettings, fnDraw) {
                    var iListLength = 5;
                    var oPaging = oSettings.oInstance.fnPagingInfo();
                    var an = oSettings.aanFeatures.p;
                    var i, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

                    if (oPaging.iTotalPages < iListLength) {
                        iStart = 1;
                        iEnd = oPaging.iTotalPages;
                    }
                    else if (oPaging.iPage <= iHalf) {
                        iStart = 1;
                        iEnd = iListLength;
                    } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
                        iStart = oPaging.iTotalPages - iListLength + 1;
                        iEnd = oPaging.iTotalPages;
                    } else {
                        iStart = oPaging.iPage - iHalf + 1;
                        iEnd = iStart + iListLength - 1;
                    }

                    for (i = 0, iLen = an.length; i < iLen; i++) {
                        // remove the middle elements
                        $('li:gt(0)', an[i]).filter(':not(:last)').remove();

                        // add the new list items and their event handlers
                        for (j = iStart; j <= iEnd; j++) {
                            sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
                            $('<li ' + sClass + '><a href="#">' + j + '</a></li>')
                                .insertBefore($('li:last', an[i])[0])
                                .bind('click', function (e) {
                                    e.preventDefault();
                                    oSettings._iDisplayStart = (parseInt($('a', this).text(), 10) - 1) * oPaging.iLength;
                                    fnDraw(oSettings);
                                });
                        }

                        // add / remove disabled classes from the static elements
                        if (oPaging.iPage === 0) {
                            $('li:first', an[i]).addClass('disabled');
                        } else {
                            $('li:first', an[i]).removeClass('disabled');
                        }

                        if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
                            $('li:last', an[i]).addClass('disabled');
                        } else {
                            $('li:last', an[i]).removeClass('disabled');
                        }
                    }
                }
            }
        });
    } catch (e) {
        console.info(e);
    }
})(window, jQuery);

/*
 * 封装网站公共table方法
 */
(function (window, $) {
    $.commonTable = function (selector, options) {
        function CommonTable(selector, options) {
            this.selector = selector;
            this.config = $.extend({}, {
                ajaxUrl: '/Common/GetList',
                columns: [],
                builds: [],
                ajaxParams: {},
                getConditions: function () { },
                beforeBuildTr: function ($tr, data) {},
                afterLoaded: function () { }
            }, options);

            this.$btnLoadMore = null;
            this.$loading = null;
            this.$tbody = $(this.selector).find('tbody');

            this.buildBtnLoad();
            this.buildLoading();
            this.loadData();
        }

        CommonTable.prototype.setTarget = function(newTableName) {
            this.config.ajaxParams.TableName = newTableName;
        };

        CommonTable.prototype.setAjaxUrl = function(newUrl) {
            this.config.ajaxUrl = newUrl;
        };

        CommonTable.prototype.changeAjaxParams = function (params) {
            for (var key in params) {
                if (params.hasOwnProperty(key)) {
                    this.config.ajaxParams[key] = params[key];
                }
            }
        };

        CommonTable.prototype.buildBtnLoad = function () {
            this.$btnLoadMore = $('<div style="text-align: center; margin-top: 15px;"><button class="btn waves-effect waves-light" style="width: 40%">加载更多</button></div>');

            var _this = this;
            _this.$btnLoadMore.on('click', function () {
                _this.config.ajaxParams.PageIndex++;
                _this.loadData();
            });

            _this.$btnLoadMore.insertAfter(_this.selector);
        };

        CommonTable.prototype.buildLoading = function () {
            this.$loading = $('<div style="margin-left: 47%;" class="preloader-wrapper big active"><div class="spinner-layer spinner-blue"><div class="circle-clipper left"><div class="circle"></div></div><div class="gap-patch"><div class="circle"></div></div><div class="circle-clipper right"><div class="circle"></div></div></div><div class="spinner-layer spinner-red"><div class="circle-clipper left"><div class="circle"></div></div><div class="gap-patch"><div class="circle"></div></div><div class="circle-clipper right"><div class="circle"></div></div></div><div class="spinner-layer spinner-yellow"><div class="circle-clipper left"><div class="circle"></div></div><div class="gap-patch"><div class="circle"></div></div><div class="circle-clipper right"><div class="circle"></div></div></div><div class="spinner-layer spinner-green"><div class="circle-clipper left"><div class="circle"></div></div><div class="gap-patch"><div class="circle"></div></div><div class="circle-clipper right"><div class="circle"></div></div></div></div>');

            var _this = this;
            _this.$loading.insertAfter(_this.selector);
        };

        CommonTable.prototype.setPageIndex = function (index) {
            this.config.ajaxParams.PageIndex = index;
        };

        CommonTable.prototype.showLoading = function () {
            this.$btnLoadMore.hide();
            this.$loading.show();
        };

        CommonTable.prototype.hideLoading = function () {
            this.$btnLoadMore.show();
            this.$loading.hide();
        };

        CommonTable.prototype.getConditions = function () {
            var _this = this,
                getConFunc = _this.config.getConditions;

            if (typeof (getConFunc) === 'function') {
                var conditions = getConFunc();
                if (conditions instanceof Array) {
                    return conditions.join('###');
                } else {
                    return conditions;
                }
            } else {
                return '';
            }
        };

        CommonTable.prototype.buildTr = function (data) {
            var _this = this;

            var $tr = $('<tr></tr>');
            if (typeof (_this.config.beforeBuildTr) === 'function') {
                _this.config.beforeBuildTr($tr, data);
            }

            _this.config.columns.forEach(function (column, index) {
                var onCreateCell;
                _this.config.builds.forEach(function (build) {
                    if (build.targets.contains(index)) {
                        onCreateCell = build.onCreateCell;
                    }
                });

                var value = data[column];
                if (typeof (value) === 'string' && value.indexOf('Date(') >= 0) {
                    value = common.processDate(value).format('yyyy-MM-dd HH:mm:ss');
                } else if(value == 'True' || value == 'False') {
                    value = value == 'True' ? true : false;
                }

                if (typeof (onCreateCell) === 'function') {
                    var result = onCreateCell(value, data);
                    if (typeof (result) === 'string' && result.indexOf('</td>') === -1) {
                        $tr.append('<td>{0}</td>'.format(result));
                    } else if (!result) {
                        $tr.append('<td>{0}</td>'.format(result));
                    } else {
                        $tr.append(result);
                    }
                } else {
                    //$tr.append('<td>{0}</td>'.format(value || ''));
                    $tr.append('<td>{0}</td>'.format(value));
                }
            });

            return $tr;
        };

        CommonTable.prototype.loadData = function () {
            var _this = this;

            if (_this.config.ajaxParams.PageIndex === 1) {
                _this.$tbody.empty();
            }

            _this.showLoading();

            var ajaxParameters = _this.config.ajaxParams,
                conditions = _this.getConditions();

            ajaxParameters.Conditions = conditions;
            common.ajax({
                url: _this.config.ajaxUrl,
                data: ajaxParameters
            }).done(function (res) {
                _this.hideLoading();

                function noMore() {
                    Materialize.toast('没有查询到结果', 3000);
                    _this.$btnLoadMore.hide();
                }

                if (res.code == 108) {
                    if (res.data instanceof Array && res.data.length > 0) {
                        res.data.forEach(function (value) {
                            var $tr = _this.buildTr(value);
                            _this.$tbody.append($tr);
                        });

                        if (res.data.length < 20) {
                            _this.$btnLoadMore.hide();
                        }

                        // after loaded
                        if ($.isFunction(_this.config.afterLoaded)) {
                            _this.config.afterLoaded();
                        }
                    } else {
                        noMore();
                    }
                } else {
                    noMore();
                }
            });
        };

        return new CommonTable(selector, options);
    };
})(window, jQuery);

/*
 * 封装input输入自动从服务器加载数据并弹出智能提示
 */
(function (window, $) {
    $.inputIntelligence = function (selector, options) {
        function Intelligence(selector, options) {
            this.config = $.extend({}, {
                ajaxUrl: '',
                ajaxParams: {},
                highlightColor: '#f50057',
                getAjaxParams: function () { },
                buildDropdownItem: function (data) { },
                afterSelected: function (data) { }
            }, options);

            this.frequency = 500; // 500ms
            this.timeoutId = 0;
            this.config.selector = selector;
            this.$input = $(this.config.selector);
            this.initDropdownList();
            this.bindEvents();
        }

        Intelligence.prototype.setDropPosition = function (dom) {
            var currentDom, domChanged, _this = this;
            if (currentDom == dom) {
                domChanged = false;
            } else {
                currentDom = dom;
                domChanged = true;
            }

            (function () {
                if (domChanged) {
                    var offsetTop = common.computeOffsetTop(currentDom),
                    offsetLeft = common.computeOffsetLeft(currentDom);

                    _this.$drop.css({
                        left: offsetLeft,
                        top: offsetTop + currentDom.clientHeight + 5,
                        width: currentDom.clientWidth,
                        padding: 10
                    });

                    _this.showDropdown();
                }
            })();
        };

        Intelligence.prototype.showDropdown = function () {
            this.$drop.css({
                opacity: 1,
                display: 'block'
            });
        };

        Intelligence.prototype.hideDropdown = function () {
            this.$drop.css({
                opacity: 0,
                display: 'none'
            });
        };

        Intelligence.prototype.bindEvents = function () {
            var _this = this;
            _this.$input.on('keyup', function () {
                var value = this.value;
                var dom = this;
                if (_this.isOverFrequency()) {
                    // 如果两次输入间隔大于等于设定的频率，则注册一个新的定时器
                    _this.timeoutId = setTimeout(function () {
                        _this.setDropPosition(dom);
                        _this.config.ajaxParams = _this.config.getAjaxParams(value);
                        _this.ajaxLoad(value);
                    }, _this.frequency);
                } else {
                    // 如果两次输入间隔小于设定的频率，则将上次注册的定时器清除，重新注册一个定时器
                    window.clearTimeout(_this.timeoutId);
                    _this.timeoutId = setTimeout(function () {
                        _this.setDropPosition(dom);
                        _this.config.ajaxParams = _this.config.getAjaxParams(value);
                        _this.ajaxLoad(value);
                    }, _this.frequency);
                }
            });
        };

        // 控制异步加载的频率
        Intelligence.prototype.isOverFrequency = function () {
            if (!this.lastTime) {
                this.lastTime = new Date();
                return true;
            } else {
                var currentTime = new Date();
                var timespan = currentTime - this.lastTime;

                this.lastTime = currentTime;
                return timespan >= this.frequency;
            }
        };

        Intelligence.prototype.ajaxLoad = function (input) {
            var _this = this;
            _this.$drop.empty();

            if (!input) {
                return;
            }

            common.ajax({
                url: _this.config.ajaxUrl,
                data: _this.config.ajaxParams
            }).done(function (res) {
                if (res.code == 108) {
                    _this.buildDropdownList(res.data, input);
                }
            });
        };

        Intelligence.prototype.initDropdownList = function () {
            var $drop = $('#___intelligence___');
            if ($drop.length > 0) {
                this.$drop = $drop;
            } else {
                this.$drop = $('<ul id="___intelligence___" style="z-index: 999999;" class="dropdown-content"></ul>');
                this.$drop.appendTo('body');
            }
        };

        Intelligence.prototype.buildDropdownList = function (data, input) {
            var _this = this;
            data.forEach(function (model) {
                var text = _this.config.buildDropdownItem(model);
                text = _this.highlighting(text, input);

                var $li = $('<li style="margin: 10px 0;">{0}</li>'.format(text));
                $li.on('click', function () {
                    _this.hideDropdown();
                    _this.$input.val($(this).text());

                    if ($.isFunction(_this.config.afterSelected)) {
                        _this.config.afterSelected(model, _this.$input);
                    }
                });

                _this.$drop.append($li);
            });
        };

        Intelligence.prototype.highlighting = function (source, target) {
            if (!!source && !!target) {
                return source.replace(target, '<font style="color: {0}">{1}</font>'.format(this.config.highlightColor, target));
            }
        };

        return new Intelligence(selector, options);
    };
})(window, jQuery);

/*
 * 根据配置信息为select元素加载数据
 */
(function (window, $) {
    $.initSelect = function (options) {
        var config = $.extend({}, {
            selectors: [],
            ajaxUrl: '',
            textField: '',
            valueField: '',
            selectedValue: '',
            getAjaxParams: function () { },
            beforeBuilt: function () { },
            afterBuilt: function () { }
        }, options);


        if (!(config.selectors instanceof Array)) {
            config.selectors = [config.selectors];
        }

        var formData = {};
        if ($.isFunction(config.getAjaxParams)) {
            formData = config.getAjaxParams() || {};
        }

        common.ajax({
            url: config.ajaxUrl,
            data: formData
        }).done(function (res) {
            var data = res.data || [];

            config.selectors.forEach(function (selector) {
                var $select = $(selector);
                if ($.isFunction(config.beforeBuilt)) {
                    config.beforeBuilt($select);
                }

                data.forEach(function (model) {
                    var value = model[config.valueField],
                        text = model[config.textField],
                        selected = value == config.selectedValue ? 'selected="selected"' : '';

                    $select.append('<option value="{0}" {1}>{2}</option>'.format(value, selected, text));
                });

                if ($.isFunction(config.afterBuilt)) {
                    config.afterBuilt($select);
                }
            });
        });
    };
})(window, jQuery);

/**
 * Materialize风格的模态框
 */
(function (window, $) {
    /**
     * 弹出Materialize风格的确定提示框
     * @param {String} title 
     * @param {String} text 
     * @param {Function} confirm 
     * @param {Function} cancel 
     * @returns {void} 
     */
    $.confirm = function (title, text, confirm, cancel) {
        var $modal = $('<div id="___modal___" class="modal"><div class="modal-content"><h4>{0}</h4><p>{1}</p></div><div class="modal-footer"><a href="javascript:void(0);" id="___confirm___" class=" modal-action modal-close waves-effect waves-green btn-flat">确 定</a><a href="javascript:void(0);" id="___cancel___" class=" modal-action modal-close waves-effect waves-green btn-flat">取 消</a></div></div>'.format(title, text)).appendTo('body');

        $modal.leanModal({
            dismissible: false, // 点击模态框外部则关闭模态框
            opacity: .5, // 背景透明度
            in_duration: 300, // 切入时间
            out_duration: 200 // 切出时间
        });

        $('#___confirm___').on('click', function () {
            if ($.isFunction(confirm)) {
                confirm();
                $('.lean-overlay').remove();
                $modal.remove();
            }
        });

        $('#___cancel___').on('click', function () {
            if ($.isFunction(cancel)) {
                cancel();
                $('.lean-overlay').remove();
                $modal.remove();
            }
        });


        $modal.openModal();
    };
})(window, jQuery);

/**
 * 为jquery.validate插件添加自定义验证方法
 */
(function(window, $) {

    if ($.validator) {
        // 为jquery.validate插件添加验证身份证的方法
        $.validator.addMethod("checkIdentity", function (value, element, param) {
            var isMatch = value == '' || /(^[1-9]\d{5}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$)/.test(value);

            return this.optional(element) || isMatch;
        }, $.validator.format("请输入有效的身份证号码"));
    }

})(window, jQuery);