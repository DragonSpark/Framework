define(["dragonspark/application", "dragonspark/configuration", "widgets/identity/signout"], function (application, configuration) {
	var instance = {};
	application.on("application:operations:refreshing").then(function (profile) {
		profile.items.unshift(
			{
				title: "Refreshing configuration and user session.",
				body: function()
				{
					$.connection.hub.stop(false, true);
					var result = $.connection.hub.start().then( configuration.refresh );
					return result;
				}
			}
		);
	}, instance);
	return instance;
});