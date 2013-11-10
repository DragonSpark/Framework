define([ "durandal/system" ], function ( system ) {
	return ko.asyncCommand({
		execute: function ( parameter ) {
			return Q.delay( 2000 ).then(function()
			{
				var message = "Hello World!  Executing command with parameter: {0}".format(parameter);
				system.log(message);
			});
		},

		canExecute: function (isExecuting) {
			return !isExecuting;
		}
	});
});