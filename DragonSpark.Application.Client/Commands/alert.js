define(function () {
	return ko.command({
		execute: function(parameter) {
			$ds.log("Hello World!  Executing command with parameter: {0}".format(parameter));
		},

		canExecute: function(isExecuting) {
			return !isExecuting;
		}
	});
});