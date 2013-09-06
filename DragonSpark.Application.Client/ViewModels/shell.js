define(["durandal/app", "dragonspark/application", "durandal/system", "dragonspark/configuration", "./navigation", "plugins/router"], function (app, application, system, configuration, navigation, router) {
	var instance = {
		isAttached: ko.observable(false),
		configuration: configuration,
		navigation: navigation,
		attached: function () {
			instance.isAttached(true);
		}
	};
	return instance;
});