define(function (require) {
	return ko.asyncCommand({
		execute: function (parameter, complete) {
			$ds.wait(1000).then(function() {
				alert("Hello World!  Executing command with parameter: {0}".format(parameter));
			}).then(complete);
		},

		canExecute: function (isExecuting) {
			return !isExecuting;
		}
	});
});