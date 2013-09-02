define(["durandal/app", "durandal/system", "plugins/widget", "./error", "./configuration", "./compensations", "./view", "plugins/dialog", "plugins/router", "durandal/events"], function (app, system, widget, error, configuration, compensations, view, dialog, router, events) {
	var dragonspark = window.$ds = {
		_refreshing : ko.observable(false),
		initialize: function () {
			dragonspark.trigger("application:initializing", this);
			return $.connection.hub.start().then( configuration.initialize ).then(function () {
				dragonspark.trigger("application:initialized", this);
			});
		},
		
		activate : function() {
			router.activate();
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

			var log = "An exception has occurred: {0}".format(dialog.details);
			$ds.log(log);

			return dialog.show(model);
		},
		
		wait: function (milliseconds) {
			return system.defer(function (dfd) {
				setTimeout( dfd.resolve, milliseconds || 2000);
			}).promise();
		}
	};

	/*var original = widget.beforeBind;
	widget.beforeBind = function (element, view, settings) {
		if (widget.beforeBind != original && (widget.beforeBind = original)) {
			widget.beforeBind = original;
			dragonspark.trigger("application:complete", this);
		}
		original(element, view, settings);
	};*/
	
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