define( [ "durandal/app", "durandal/system", "durandal/events", "durandal/viewLocator", "./configuration", "./error", "plugins/serializer", "plugins/widget", "plugins/dialog", "plugins/router", "plugins/server" ], 
function ( app, system, events, viewLocator, configuration, error, serializer, widget, dialog, router, server ) 
{
	function supportsLocalStorage() 
	{
		try 
		{
			return 'localStorage' in window && window['localStorage'] !== null;
		}
		catch (e) 
		{
			return false;
		}
	}
	
	function ensure()
	{
		var enabled = navigator.cookieEnabled ? true : false;
		if ( typeof navigator.cookieEnabled == "undefined" && !enabled )
		{
			document.cookie = "testcookie";
			enabled = document.cookie.indexOf( "testcookie" ) != -1 ? true : false;
		}
		
		if ( !enabled )
		{
			throw new Error( "Cookies are not enabled.  Please ensure cookies are enabled." );
		}
		
		if ( !supportsLocalStorage() )
		{
			throw new Error( "Local HTML5 Storage not detected.  Please use a HTML5-supported browser." );
		}
	}
	
	function refresh()
	{
		var shell = configuration().Navigation.Shell;
		// app.setRoot( shell.moduleId );
		dragonspark.activate().then( function() {
			app.setRoot( shell.moduleId );
			dragonspark.trigger( "application:refreshed", this );
		} );
	}
	
	system.defer = function (action) {
        var deferred = Q.defer();
		var method = action || function () {};
        method.call( deferred, deferred );
        var promise = deferred.promise;
        deferred.promise = function () {
            return promise;
        };
        return deferred;
    };

	var dragonspark = window.$ds = 
	{
		initialize: function ()
		{
			ensure();
			dragonspark.trigger( "application:initializing", this );
			return system.acquire( "dragonspark/navigation" ).then( server.initialize ).then(function () {
				
				configuration.on( "application:configuration:refreshed", refresh );
				
				with ( configuration() )
				{
					system.debug(EnableDebugging);
					
					system.error = function( e )
					{
						var args = { exception: dragonspark.determineError( e ), handled : false };
						dragonspark.trigger( "application:exception", args );

						if ( !args.handled )
						{
							var model = new error( args.exception );

							var log = "An exception has occurred: {0}".format( model.details );
							system.log( log );
			
							dialog.show( model );
						}
					};

					var initializers = Enumerable.From(Initializers),
						commands = Enumerable.From(Commands),
						widgets = Enumerable.From(Widgets);

					widgets.ToArray().map(function (i) {
						widget.mapKind(i.Name, i.ViewId, i.Path);
					});

					var modules = commands.Concat( initializers ).Where( "$.Path != null" ).Select( "$.Path" ).ToArray();

					return system.acquire( modules ).then( function () {
						app.title = ApplicationDetails.Title;
						app.configurePlugins( { router: true, dialog: true, widget: true } );
						
						ko.validation.configure( {
							registerExtenders: true,
							messagesOnModified: true,
							insertMessages: false,
							parseInputAttributes: true,
							messageTemplate: null
						} );
					});
				}
			}).then( server.start ).then( function() {
				return app.start().then(function () {
					viewLocator.useConvention();

					var shell = configuration().Navigation.Shell;
					app.setRoot( shell.moduleId, shell.transitionName );
					dragonspark.trigger( "application:initialized" );
				});
			});
		},
		
		activate : function() {
			return router.activate();
		},
		
		chain: function (f1, f2, context) {
			return function () {
				if (f1) {
					f1.apply(context, arguments);
				}	
				if (f2) {
					f2.apply(context, arguments);	
				}
			};
		},
		
		refresh: function ()
		{
			dragonspark.trigger("application:refresh", this);
		},
		
		determineError: function( e )
		{
			var exception = e instanceof Error ? e : new Error( e );
			
			exception.stack = exception.stack || printStackTrace( exception ).splice( 5 ).join( "\n" );
			return exception;
		}
	};

	window.onerror = function ( e )
	{
		var item = dragonspark.determineError( e );
		system.error( item );
		return true;
	};

	events.includeIn(dragonspark);

	function asDeserializer( ctor )
	{
		var result = function( item )
		{
			var object = new ctor( item.value );
			return object;
		};
		return result;
	}

	$.ajaxSettings.converters[ "text json" ] = serializer.deserialize.bind( serializer );
	serializer.registerType( "System.DateTime", asDeserializer( Date ) );
	// serializer.registerType( "System.Version", asDeserializer( Version ) );

	return dragonspark;
});

/*function Version(version) {
	this.major = version.Major;
	this.minor = version.Minor;
	this.build = version.Build;
	this.revision = version.Revision;

	this.toString = function() {
		var result = "{0}.{1}.{2}.{3}".format(this.major, this.minor, this.build, this.revision);
		return result;
	};
}*/

function FatalError(message) {
	this.message = message;
}
FatalError.prototype = new Error;

function ServiceError(message) {
	this.message = message;
	this.stack = "None Available.";
}
ServiceError.prototype = new Error;

if (!String.prototype.format) {
	String.prototype.format = function () {
		var args = arguments;
		return this.replace(/{(\d+)}/g, function (match, number) {
			return typeof args[number] != 'undefined' ? args[number] : match;
		});
	};
}
	
Array.prototype.remove = function(from, to) {
  var rest = this.slice((to || from) + 1 || this.length);
  this.length = from < 0 ? this.length + from : from;
  return this.push.apply(this, rest);
};


if (typeof Object.clone !== 'function') {
	Object.clone = function (o) {
		var clone = function () { };
		clone.prototype = o;
		return new clone();
	};
}