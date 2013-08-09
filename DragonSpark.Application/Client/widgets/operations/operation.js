define(["durandal/system", "./viewmodel.initialize", "require"], function (system, module, r) {
	var ctor = function (monitor, settings) {
		this.monitor = monitor;
		this.settings = $.extend({
			title: "Loading... please wait.",
			exceptionHandlingAction: module.ExceptionHandlingAction.None
		}, settings);

		this.title = ko.observable(this.settings.title);
		this.status = ko.observable(module.ExecutionStatus.None);
		this.exceptionHandlingAction = ko.observable(this.settings.exceptionHandlingAction);
		this.exception = ko.observable();
	};

	ctor.prototype.determine = function(path) {
		var url = r.toUrl(path);
		return url;
	};
	
	ctor.prototype.reset = function() {
	},
	
	ctor.prototype.execute = function (parameter) {
		function determinePromise(settings) {
			try {
				if (settings.path !== undefined) {
					var command = require(settings.path());
					var result = command.isExecuting ? system.defer(function(dfd) {
						var sub = command.isExecuting.subscribe(function(done) {
							if (!done) {
								dfd.resolve();
								sub.dispose();
							}
						});
					}).promise() : null;
					command.execute(settings.parameter || parameter, result);
					return result;
				} else if (settings.body) {
					var parameters = [settings.parameter || parameter];
					return settings.body.apply( parameter.settings.bindingContext.$data, parameters);
				}
				return null;
			} catch (e) {
				return e;
			} 
		}

		this.exception(null);
		this.status(module.ExecutionStatus.Executing);
		
		var self = this;
		return system.defer(function (dfd) {
			var error = function(e) {
				self.status(module.ExecutionStatus.CompletedWithException);
				self.exception(e);
				dfd.reject(e);
			};
			var complete = function (context) {
				self.status(module.ExecutionStatus.Completed);
				dfd.resolve();
			};
			var promise = determinePromise(self.settings);
			if (promise instanceof Error) {
				error(promise);
			} else if (promise) {
				$.when( promise ).then(complete, error);
			} else {
				complete();
			}
		}).promise();
	};

	ctor.prototype.abort = function() {

	};

	return ctor;
});