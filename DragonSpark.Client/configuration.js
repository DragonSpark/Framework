define(["durandal/app", "durandal/events"], function (app, events) {
	function refresh(data) {
		instance.trigger("application:configuration:refreshing", instance, data);
		instance(data);

		with (data) {
			// TODO: This is dumb.
			ApplicationDetails.Version = new Version( ApplicationDetails.Version );
			ApplicationDetails.DeploymentDate = new Date( ApplicationDetails.DeploymentDate );
		}
		instance.trigger("application:configuration:refreshed", instance, data);
	}
	
	var instance = $.extend( ko.observable(), {
		apply: refresh
	});

	events.includeIn(instance);
	return instance;
});