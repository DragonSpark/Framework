define( [ "durandal/app", "durandal/system", "durandal/events", "durandal/viewLocator", "plugins/widget", "plugins/dialog", "plugins/router", "plugins/server", "./navigation", "./configuration", "./error" ], 
function ( app, system, events, viewLocator, widget, dialog, router, server, navigation, configuration, error ) 
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
			return server.initialize().then(function () {
				ko.validation.configure( {
					registerExtenders: true,
					messagesOnModified: true,
					insertMessages: false,
					parseInputAttributes: true,
					messageTemplate: null
				} );

				configuration.on( "application:configuration:refreshed", refresh );
				
				with ( configuration() )
				{
					system.debug(EnableDebugging);

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
		
		log: function (message) { console.log(message); },
		
		throw: function (e) {
			var model = new error(e);

			var log = "An exception has occurred: {0}".format(model.details);
			$ds.log(log);
			
			return dialog.show(model);
		},
		
		wait: function (milliseconds) {
			return system.defer(function (dfd) {
				setTimeout( dfd.resolve, milliseconds || 2000);
			}).promise();
		}
	};

	window.onerror = function (message) {
		var e = new Error(message);
		e.trace = Enumerable.From(arguments.callee.trace()).Where("$.params.length").Select("	'	at {0} in {1}:line {2}'.format($.name, $.params[1], $.params[2])").Where("$ != ''").ToArray().join("\n");
		var result = $ds.throw(e);
		return result;
	};

	events.includeIn(dragonspark);
	
	return dragonspark;
});

function Version(version) {
	this.major = version.Major;
	this.minor = version.Minor;
	this.build = version.Build;
	this.revision = version.Revision;

	this.toString = function() {
		var result = "{0}.{1}.{2}.{3}".format(this.major, this.minor, this.build, this.revision);
		return result;
	};
}

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
	
ko.observableArray.fn.pushAll = function (valuesToPush) {
	var underlyingArray = this();
	this.valueWillMutate();
	ko.utils.arrayPushAll(underlyingArray, valuesToPush);
	this.valueHasMutated();
	return this;  //optional
};

/*if (typeof Object.getTypeName !== 'function') {
	Object.prototype.getTypeName = function () {
		var funcNameRegex = /function (.{1,})\(/;
		var results = (funcNameRegex).exec((this).constructor.toString());  // 
		return (results && results.length > 1) ? results[1] : "";
	};
}*/

if (typeof Object.clone !== 'function') {
	Object.clone = function (o) {
		var clone = function () { };
		clone.prototype = o;
		return new clone();
	};
}