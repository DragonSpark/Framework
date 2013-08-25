define(["durandal/app", "dragonspark/configuration", "./navigation"], function (app, configuration, navigation) {
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
	});
	return instance;
});