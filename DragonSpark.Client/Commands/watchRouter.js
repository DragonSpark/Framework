define( [ "durandal/system", "plugins/router" ], function ( system, router ) 
{
	return ko.asyncCommand( {
		execute: function( item, callback )
		{
			var target = item || router;
			var context = this;
			var defer = system.defer( function(dfd) {
				target.on( "router:navigation:composition-complete", function() {
					dfd.resolve();
					target.off( "router:navigation:composition-complete", null, context );
				}, context );
			}, context ).promise().finally(callback);
			return defer;
		},

		canExecute: function(isExecuting) {
			return !isExecuting;
		}
	} );
});