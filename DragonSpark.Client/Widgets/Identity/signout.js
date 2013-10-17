define(["dragonspark/application", "dragonspark/session"], function (application, service) {
	return ko.asyncCommand({
		execute: function (parameter) {
			return Q(service.invoke( "SignOut" )).then(application.refresh);
		},

		canExecute: function (isExecuting) {
			return !isExecuting;
		}
	});
});