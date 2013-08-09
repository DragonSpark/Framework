define(function (require) {
	return ko.asyncCommand({
		execute: function (parameter, complete) {
			$ds.wait(2000).then(function() {
				$ds.log("Hello World!  Executing command with parameter: {0}".format(parameter));
			}).then(complete);
		},

		canExecute: function (isExecuting) {
			return !isExecuting;
		}
	});
});