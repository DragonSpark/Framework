define([ "durandal/system" ], function ( system ) {
	return ko.asyncCommand({
		execute: function (parameter) {
			return Q.delay(2000).then(function() {
				system.log("Hello World!  Executing command with parameter: {0}".format(parameter));
			});
		},

		canExecute: function (isExecuting) {
			return !isExecuting;
		}
	});
});