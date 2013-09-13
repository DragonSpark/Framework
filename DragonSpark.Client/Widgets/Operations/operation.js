define(["durandal/system", "./core", "require"], function (system, core, r) {
	var ctor = function (monitor, settings) {
		this.monitor = monitor;
		this.settings = $.extend({
			title: "Loading... please wait.",
			exceptionHandlingAction: core.ExceptionHandlingAction.None
		}, settings);

		this.title = ko.observable(this.settings.title);
		this.status = ko.observable(core.ExecutionStatus.None);
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
			try 
			{
				if (settings.path !== undefined)
				{
					var result = system.acquire( settings.path() ).then( function( command ) {
						var promise = command.isExecuting ? system.defer(function(dfd) {
						var sub = command.isExecuting.subscribe(function(done) {
								if (!done) {
									dfd.resolve();
									sub.dispose();
								}
							});
						}).promise() : null;
						command.execute(settings.parameter || parameter, result);
						return promise;
					} );
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
		this.status(core.ExecutionStatus.Executing);
		
		var self = this;
		return system.defer(function (dfd) {
			var error = function(e)
			{
				self.status(core.ExecutionStatus.CompletedWithException);
				
				// HACK: SignalR returns messages and not errors, lesigh.
				var serviceError = typeof e == "string" && $ds.lastServiceError;
				var exception = serviceError ? new ServiceError( $ds.lastServiceError.Message ) : e;
				$ds.lastServiceError = serviceError ? null : $ds.lastServiceError;
				
				self.exception(exception);
				dfd.reject(exception);
			};
			var complete = function (context) {
				self.status(core.ExecutionStatus.Completed);
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