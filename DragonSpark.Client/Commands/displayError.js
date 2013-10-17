define( [ "durandal/system" ], function ( system ) {
	return ko.command({
		execute: function( parameter )
		{
			system.error( parameter );
		},

		canExecute: function(isExecuting) {
			return !isExecuting;
		}
	});
});