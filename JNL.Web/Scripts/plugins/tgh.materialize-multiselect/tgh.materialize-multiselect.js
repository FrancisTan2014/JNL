/**
 * @description Material风格的multi select插件，依赖materializM.css
 *				目前本插件中的选项数据需要动态加载
 * @author FrancisTan
 * @since 2016-10-14  
 */
(function (factory) {
	if (typeof window.define === 'function') {
		if (window.definM.amd) {
			// AMD模式
			window.define('Multiselect', ["jquery"], factory);
		} else if (window.definM.cmd) {
			// CMD模式
			window.define(function (require, exports, module) {
				return factory;
			});
		} else {
			// 全局模式
			factory(window.jQuery);
		}
	} else if (typeof module === "object" && typeof modulM.exports === "object") {
		// commonjs
		modulM.exports = factory(
            require('./jquery.js')
        );
	} else {
		// 全局模式
		factory(window.jQuery);
	}
})(function ($) {
	// 验证是否引用jquery
	if (!$ || !$.fn || !$.fn.jquery) {
		throw new Error('本插件依赖jquery 1.9.0以上的版本，请导入jquery！');
	}

	// 定义扩展函数
	var _extend = function (fn) {
		var M = window.Multiselect;
		if (M) {
			// 执行传入的函数
			fn(M, $);
		}
	};

	// 定义构造函数
	(function (window, $) {

		if (window.Multiselect) {
			// 重复引用
			consolM.warn('检测到重复引用！！！');
			return;
		}

		// 构造函数
		var M = function (elemId, options) {

			// ---------------获取基本节点------------------
			var $elem = $('#' + elemId);
			if ($elem.length !== 1) {
				return;
			}

			this.config = $.extend({}, this.config, options);

			this.$elem = $elem;
			this.$parent = $elem.parent();
		    this.$document = $(document);

			// 选中的项 { value: any, text: any }
			this.selected = [];

			// 为插件加载的数据
			this.data = [];

			// 列表可见状态
			this.isListVisible = false;

			// ------------------初始化------------------
			this.init();
		};

		M.fn = M.prototype;

		// 暴露给全局对象
		window.Multiselect = M;
	})(window, $);

	// 初始化
	_extend(function (M, $) {

		M.fn.init = function () {

			this.createTrigger();
			this.bindTriggerEvents();

			this.createList();

			this.$elem.hide();
			this.createMultiselect();

			this.config.loadData(this.afterDataLoaded, this);

			// 当点击列表之外的地方时隐藏列表
		    this.hideWhenClickOutside();
		};
	});

	// 创建Multiselect
	_extend(function (M, $) {

		M.fn.createMultiselect = function () {

			this.$container = $('<div class="material-multiselect"></div>');

			this.$container.append(this.$trigger);

			// 右侧向下小箭头
			this.$container.append('<i class="mdi-hardware-keyboard-arrow-down material-multiselect-down"></i>');

			this.$container.append(this.$list);

		    this.$parent.append(this.$container);
		};
	});

	// 全局配置
	_extend(function (M, $) {

		M.config = {};

		// 插件在没有选中任何项时显示的文字提示
		M.config.placeholder = '多项选择';

		// 为插件加载数据的方法，在数据加载完成后务必调用回调函数
		// 并将加载到的数据做为参数传递给回调函数
		M.config.loadData = function (callback) { };

		// 将从加载的数据中提取出显示文字的键
		M.config.textKey = '';

		// 将从加载的数据中提取出选中后的值的键
		M.config.valueKey = '';
	});

	// 创建选项
	_extend(function (M, $) {

		// 创建选项元素li，返回表示li元素的jQuery对象
		M.fn.createOption = function (data) {

			var text = data[this.config.textKey],
		        value = data[this.config.valueKey];

			return $('<li data-value="' + value + '" data-text="' + text + '"><input type="checkbox" /><label>' + text + '</label></li>');
		};
	});

	// 创建插件触发元素（一个input元素）
	_extend(function (M, $) {

		M.fn.createTrigger = function () {

			var html = '<input type="text" placeholder="' + this.config.placeholder + '" class="material-multiselect-input" readonly="readonly">';

			this.$trigger = $(html);
		};
	});

	// 为trigger绑定事件
	_extend(function (M, $) {

		M.fn.bindTriggerEvents = function () {

			var _this = this;
			_this.$trigger.on('click', function () {

				if (!_this.isListVisible) {
				    _this.showList();
				} else {
				    _this.hideList();
				}
			});

		};
	});

	// 创建选择下拉列表Ul元素
	_extend(function (M, $) {

		M.fn.createList = function () {

			var html = '<ul class="dropdown-content"></ul>';

		    this.$list = $(html);
		};
	});

	// 显示下拉列表
	_extend(function (M, $) {

		M.fn.showList = function () {

			if (!this.isListVisible) {
				this.isListVisible = true;
				this.$list.addClass('visible');
			}
		};
	});

	// 隐藏下拉列表
	_extend(function (M, $) {

		M.fn.hideList = function () {

			this.isListVisible = false;
			this.$list.removeClass('visible');
		};
	});

	// 数据加载完成后保存data
	_extend(function (M, $) {

		// 注意：由于此方法不是在插件内部调用，因此在配置插件
		//      的loadData方法时，请将第二个参数原封不动地传
		//      给此回调方法
		M.fn.afterDataLoaded = function (data, multiObj) {

			multiObj.data = data instanceof Array ? data : [];
			multiObj.fillList(multiObj.data);
		};
	});

	// 将数据填充到下拉列表中
	_extend(function (M, $) {

		M.fn.fillList = function (data) {

			var _this = this;
			data.forEach(function (item) {
				var $li = _this.createOption(item);
				$li.on('click', function () {
					_this.optionClick(this);
				});

				_this.$list.append($li);
			});

		};
	});

	// 选项点击事件
	_extend(function (M, $) {

		M.fn.optionClick = function (dom) {

			var $dom = $(dom),
                value = $dom.data('value'),
                text = $dom.data('text');

			// 切换样式
			if ($dom.hasClass('active')) {
			    $dom.removeClass('active');
			    this.removeSelect(value, text);
			} else {
				$dom.addClass('active');
			    this.addSelect(value, text);
			}

			// 改变checkbox选中状态
		    var $checked = $dom.find('[type=checkbox]'),
		        isChecked = $checked.prop('checked');
		    $checked.prop('checked', !isChecked);

			// 设置提示框的文本
		    this.setTriggerValue();
		};
	});

	// 设置$trigger的提示文本
    _extend(function(M, $) {

        M.fn.setTriggerValue = function() {

            var selectedCount = this.selected.length,
                text = this.selected.length === 0
                    ? ''
                    : '已选择' + selectedCount + '项';

            this.$trigger.val(text);
        };
    });

	// 将指定值从选中项数组中移除(只移除匹配到的第一个)
	_extend(function (M, $) {

		M.fn.removeSelect = function (value, text) {

			var _this = this;
			_this.selected.forEach(function (item, i) {
				if (item.value == value && item.text == text) {
					_this.selected.splice(i, 1);
					return false;
				}
		    });
		};
	});

	// 将选中项添加至选中项数组中
    _extend(function(M, $) {

        M.fn.addSelect = function(value, text) {

            this.selected.push({
                value: value,
                text: text
            });
        };
    });
    
	// 绑定document点击事件，当点击列表之外的地方时隐藏列表
    _extend(function(M, $) {

        M.fn.hideWhenClickOutside = function() {

        	var _this = this;
        	_this.$document.on('click', function(e) {

            	var $target = $(e.target);

            	if (e.target == _this.$list[0] || $target.parents('ul')[0] == _this.$list[0] ||
					e.target == _this.$trigger[0]) {

	                // 点击了列表内的元素，不做响应
            		return;

            	} else {

            		// 点击了页面上其他元素，将列表隐藏
	                _this.hideList();
	            }

            });
        };
    });
});