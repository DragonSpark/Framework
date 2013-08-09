define(function (require) {
	if (!String.prototype.format) {
		String.prototype.format = function () {
			var args = arguments;
			return this.replace(/{(\d+)}/g, function (match, number) {
				return typeof args[number] != 'undefined' ? args[number] : match;
			});
		};
	}
	
	ko.observableArray.fn.pushAll = function (valuesToPush) {
		var underlyingArray = this();
		this.valueWillMutate();
		ko.utils.arrayPushAll(underlyingArray, valuesToPush);
		this.valueHasMutated();
		return this;  //optional
	};

	/*if (typeof Object.getTypeName !== 'function') {
		Object.prototype.getTypeName = function () {
			var funcNameRegex = /function (.{1,})\(/;
			var results = (funcNameRegex).exec((this).constructor.toString());  // 
			return (results && results.length > 1) ? results[1] : "";
		};
	}*/

	if (typeof Object.clone !== 'function') {
		Object.clone = function (o) {
			var clone = function () { };
			clone.prototype = o;
			return new clone();
		};
	}
});

