define( [ "dragonspark/navigation" ], function ( navigation ) {
	var instance = 
	{
		route: ko.observable(),
		activate: function()
		{
			// var item = route == "notfound" && navigation.items.length > 1 ? navigation.items[1] : route || "Not Found";
			var lastAttemptedFragment = navigation.lastAttemptedFragment();
			instance.route( lastAttemptedFragment );
		}
	};

	return instance;
} );