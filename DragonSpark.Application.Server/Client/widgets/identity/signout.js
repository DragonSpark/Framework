define(["dragonspark/configuration", "dragonspark/service"], function (configuration, service) {
	return ko.asyncCommand({
		execute: function (parameter, complete) {
			return service.invoke("SignOut").then(configuration.refresh).always(complete);
		},

		canExecute: function (isExecuting) {
			return !isExecuting;
		}
	});
});