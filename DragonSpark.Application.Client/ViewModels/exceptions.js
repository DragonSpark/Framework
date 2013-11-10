define( [ "durandal/system", "plugins/server" ], function ( system, server ) {
	var instance =
	{
		clientException: function ()
		{
			var error = new Error("This is an error thrown on the client side.  You may report this exception to the server using the button below.");
			system.error(error);
		},

		serverException: function ()
		{
			return server.hubs.application.server.throw();
		}
	};
	return instance;
});