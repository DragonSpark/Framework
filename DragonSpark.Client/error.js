define(["dragonspark/configuration", "dragonspark/view", "plugins/dialog", "plugins/server"], function (configuration, view, modal, server)
{
	var ctor = function (error) {
		var instance = configuration(),
			applicationDetails =  instance ? instance.ApplicationDetails : null,
			self = this,
			details = { Title: applicationDetails ? applicationDetails.Title : "N/A", Product: applicationDetails ? applicationDetails.Product : "N/A", VersionInformation: applicationDetails ? applicationDetails.Version : "N/A" };
		
		this.error = error;
		this.details = "Exception occured in application {1} ({2}).{0}[Version: {3}]{0}{0}Message: {4}{0}{0}StackTrace Information Details:{0}======================================{0}{5}".format(
			"\n",
			details.Title,
			details.Product,
			details.VersionInformation,
			error.message,
			error.stack || "Not available.");

		this.view = view;

		this.reported = ko.observable(false);

		this.allowReporting = ko.computed(function() {
			return !(error instanceof ServiceError) && !self.reported();
		});
	};

	ctor.prototype.close = function() {
		modal.close(this);
	};

	ctor.prototype.report = function () {
		var self = this;
		var payload = { Message: this.error.message, StackTrace: this.error.stack, ClientExceptionType: this.error.name };
		return Q(server.hubs.application.server.reportException( payload )).fin(function() {
			self.reported(true);
		});
	};

    return ctor;
});