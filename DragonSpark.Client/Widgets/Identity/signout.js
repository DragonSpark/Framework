define(["dragonspark/application", "dragonspark/session"], function (application, service) {
	return ko.asyncCommand({
		execute: function (parameter, complete) {
			return Q(service.invoke( "SignOut" )).then(application.refresh).fin(complete);
		},

		canExecute: function (isExecuting) {
			return !isExecuting;
		}
	});
});