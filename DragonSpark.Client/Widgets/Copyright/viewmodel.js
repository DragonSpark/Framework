define([ "durandal/system", "durandal/app", "./about", "dragonspark/configuration", "plugins/dialog"], function (system, app, about, configuration, dialog ) {
	var details = configuration().ApplicationDetails;
	
	return {
		details: details,
		deployment: ko.observable(details.DeploymentDate.getFullYear()),
		sameYear: ko.observable(new Date(Date.now()).getFullYear() == details.DeploymentDate.getFullYear()),
		version: ko.observable(details.Version.toString()),
		about: function () {
			var model = new about(details, configuration().UserProfile);
			dialog.show( model );
		}
	};
});