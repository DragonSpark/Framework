define(["dragonspark/configuration", "./navigation", "plugins/router"], function (configuration, navigation, router) {
	var model = {};

	configuration.activateRouter( model );

	return model;
});