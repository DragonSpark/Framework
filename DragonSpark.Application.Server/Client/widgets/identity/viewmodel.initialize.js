define(["dragonspark/application", "dragonspark/configuration", "./signout"], function (application, configuration) {
	var instance = {};
	application.on("application:operations:refreshing").then(function (profile) {
		profile.items.pushAll([
			{
				title: "Refreshing configuration and user session.",
				body: configuration.refresh,
			}
		]);
	}, instance);
	return instance;
});