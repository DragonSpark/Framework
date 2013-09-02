define(["dragonspark/application", "dragonspark/service"], function (application, service) {
	return ko.asyncCommand({
		execute: function (parameter, complete) {
			return service.invoke( "SignOut" ).then(application.refresh).always(complete);
		},

		canExecute: function (isExecuting) {
			return !isExecuting;
		}
	});
});