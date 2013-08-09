define(function (require) {
	return ko.command({
		execute: function(parameter) {
			throw parameter;
		},

		canExecute: function(isExecuting) {
			return !isExecuting;
		}
	});
});