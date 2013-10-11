define(["dragonspark/application", "plugins/server", "widgets/identity/signout"], function (application, server) {
	var instance = {};
	application.on( "application:operations:refreshing", function ( profile ) {
		profile.items.unshift( {
			title: "Refreshing configuration and user session.",
			body: server.restart
		} );
	} );
	return instance;
});