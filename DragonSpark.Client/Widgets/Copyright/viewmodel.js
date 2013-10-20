define([ "durandal/system", "durandal/app", "./about", "dragonspark/configuration", "plugins/dialog"], function (system, app, about, configuration, dialog ) {
	var details = configuration().ApplicationDetails;
	
	var year = new Date(Date.now()).getFullYear();
	var deployment = details.DeploymentDate.getFullYear();
	return {
		year: year,
		deployment: deployment,
		details: details,
		about: function () {
			var model = new about(details, configuration().UserProfile);
			dialog.show( model );
		}
	};
});