define( [ "durandal/system" ], function ( system ) {
	return ko.command({
		execute: function(parameter) {
			system.log("Hello World!  Executing command with parameter: {0}".format(parameter));
		},

		canExecute: function(isExecuting) {
			return !isExecuting;
		}
	});
});