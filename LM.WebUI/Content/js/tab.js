
// Tab «–ªª
$.fn.tabs = function (options) {
	var defaults = {
		t: "",
		c: "",
		s: "current",
		m: "mouseover",
		i: 0
	}
	var options = $.extend(defaults, options);
	this.each(function () {
		var obj = $(this);
		//≥ı ºªØœ‘ æ
		$(options.t, obj).eq(options.i).addClass(options.s);
		$(options.c, obj).eq(options.i).show().siblings().hide();
		//«–ªª
		var _index;
		$(options.t, obj).bind(options.m, function () {
			$(this).addClass(options.s).siblings().removeClass(options.s);
			_index = $(options.t, obj).index($(this));
			$(options.c, obj).eq(_index).show().siblings().hide();
		});
	});
}