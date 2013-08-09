define(["dragonspark/service", "dragonspark/configuration", "dragonspark/view", "plugins/dialog"], function (service, configuration, view, modal) {
		
	var ctor = function (error) {
		var instance = configuration();
		var self = this,
			details = { Title: instance.ApplicationDetails.Title, Product: instance.ApplicationDetails.Title, VersionInformation: instance.ApplicationDetails.Version };
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
		return service.call( "Report", payload).always(function() {
			self.reported(true);
		});
	};

    return ctor;
});

Function.prototype.trace = function() {
	var trace = [];
	var current = this;
	while (current) {
		trace.push(current.signature());
		current = current.caller;
	}
	return trace;
};

Function.prototype.signature = function() {
	var signature = {
		name: this.getName(),
		params: [],
		toString: function() {
			var params = this.params.length > 0 ?
				"'" + this.params.join("', '") + "'" : "";
			return this.name + "(" + params + ")";
		}
	};

	if (this.arguments) {
		for (var x = 0; x < this.arguments.length; x++)
			signature.params.push(this.arguments[x]);
	}
	return signature;
};

Function.prototype.getName = function() {
	if (this.name)
		return this.name;
	var definition = this.toString().split("\n")[0];
	var exp = /^function ([^\s(]+).+/;
	if (exp.test(definition))
		return definition.split("\n")[0].replace(exp, "$1") || "anonymous";
	return "anonymous";
};
