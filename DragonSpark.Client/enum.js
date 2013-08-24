define(function() {
	var ctor = function (values) {
		var index = 0;
		var self = this;
		$.each(values, function(key, value) {
			self[key] = $.extend({ value: index++, name: key, description: key }, value);
		});
	};
	return ctor;
});