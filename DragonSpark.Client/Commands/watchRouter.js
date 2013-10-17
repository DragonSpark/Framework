define( [ "durandal/system", "dragonspark/application", "dragonspark/navigation" ], function ( system, application, navigation ) {
	function check( dfd, resolve )
	{
		var exception = navigation.exception;
		if ( exception )
		{
			dfd.reject( exception );
			navigation.exception = null;
		} 
		else if ( resolve )
		{
			dfd.resolve();
		}
		return resolve || exception != null;
	}
	var instance = ko.asyncCommand( {
		execute: function()
		{
			var context = this;

			return system.defer( function( dfd ) {
				var dfd1 = !navigation.isActive();
				if ( !check( dfd, dfd1 ) )
				{
					navigation.on( "navigation:complete", function() {
						navigation.off( "navigation:complete", null, context );
						check( dfd, true );
					}, context );
				}
			}, context ).promise();
		},

		canExecute: function(isExecuting) {
			return !isExecuting;
		}
	} );
	return instance;
} );