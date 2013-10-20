define( [ "durandal/system", "./core", "require" ], function ( system, core, r ) {
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

	ctor.prototype.determine = function( path ) {
		var url = r.toUrl(path);
		return url;
	};
	
	ctor.prototype.reset = function() {},
	
	ctor.prototype.execute = function( parameter )
	{
		function determinePromise( settings ) {
			try 
			{
				if ( settings.path !== undefined )
				{
					var result = system.acquire( settings.path ).then( function( command ) {
						var execute = command.execute( settings.parameter );
						return execute;
					} );
					return result;
				} else if (settings.body) {
					var parameters = [settings.parameter];
					var apply = settings.body.apply( parameter.settings.bindingContext.$data, parameters);
					return apply;
				}
				return null;
			} catch (e) {
				return e;
			} 
		}

		this.exception( null );
		this.status( core.ExecutionStatus.Executing );
		
		var self = this;
		
		return system.defer( function ( dfd )
		{
			var complete = function () {
				self.status(core.ExecutionStatus.Completed);
				dfd.resolve();
			};
			
			var error = function(e)
			{
				self.status(core.ExecutionStatus.CompletedWithException);
				
				var serviceError = e.source == "Exception" || e.status == 500 || ( e.message && e.message.message );
				var exception = serviceError ? new ServiceError( e ) : e;
				self.exception( exception );
				dfd.reject( exception );
			};

			var promise = determinePromise( self.settings );
			
			if ( promise instanceof Error )
			{
				error( promise );
			} 
			else if ( promise )
			{
				return Q( promise ).then( complete, error );
			}
			else
			{
				complete();
			}
			return null;
		}).promise();
	};

	ctor.prototype.abort = function() {};

	return ctor;
});