define(["durandal/app", "durandal/system", "dragonspark/configuration", "./navigation", "plugins/router"], function (app, system, configuration, navigation, router) {
	var instance = {
		isAttached: ko.observable(false),
		configuration: configuration,
		navigation: navigation,
		attached: function () {
			instance.isAttached(true);
		}
	};
	configuration.on("application:configuration:refreshed", function () {
		app.setRoot(instance);
		var fragment = router.activeInstruction().fragment;
		router.loadUrl( fragment );
	});
	return instance;
});