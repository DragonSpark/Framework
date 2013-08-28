define(["service"], function ( service ) {
	var instance = {
		clientException : function() {
			$ds.throw(new Error("This is an error thrown on the client side.  You may report this exception to the server using the button below."));
		},
		serverException : function () {
			return service.throw();
		}
	};
	return instance;
});