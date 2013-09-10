define( [ "durandal/system", "durandal/router" ], function ( system, router ) 
{
	return ko.command( {
		execute: function( item )
		{
			var target = item || router;
			var context = {};
			var defer = system.defer( function(dfd) {
				target.on( "router:navigation:cancelled router:navigation:complete", function() {
					dfd.resolve();
					target.off( "router:navigation:cancelled router:navigation:complete", null, context );
				} );
			}, context );
			return defer;
		},

		canExecute: function(isExecuting) {
			return !isExecuting;
		}
	} );
});