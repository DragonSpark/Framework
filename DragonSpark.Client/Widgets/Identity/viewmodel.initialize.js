﻿define(["dragonspark/application", "dragonspark/configuration", "widgets/identity/signout"], function (application, configuration) {
	alert( 124 );
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