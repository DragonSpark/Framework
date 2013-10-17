define(["dragonspark/application", "plugins/server"], function (application, server) {
	var instance = {};
	application.on( "application:operations:refreshing", function ( profile ) {
		profile.items.unshift( {
			title: "Refreshing user identity.",
			body: server.restart
		} );
	} );
	return instance;
});