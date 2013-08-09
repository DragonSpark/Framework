define(function (require) {
	return ko.command({
		execute: function(parameter) {
			$ds.throw( parameter );
		},

		canExecute: function(isExecuting) {
			return !isExecuting;
		}
	});
});