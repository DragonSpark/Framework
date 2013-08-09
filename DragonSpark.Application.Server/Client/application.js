requirejs.config({
	paths: {
		text: "vendor/text",
		durandal: "durandal",
		plugins: "plugins",
		transitions: "durandal/transitions",
		widgets: "widgets"
	}
});

define('jquery', [], function () { return jQuery; });
define('knockout', [], function () { return ko; });

define(["dragonspark/application"], function (dragonSpark) {
	dragonSpark.initialize();
});