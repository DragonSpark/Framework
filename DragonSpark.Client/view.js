define(function (require) {
	ko.bindingHandlers.initialize = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			instance.ensureAttached(element, function () {
				var method = valueAccessor();
				method();
			});
		},
		update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) { }
	};	
	
	ko.bindingHandlers.validation = {
		init : function( element, valueAccessor, allBindingsAccessor, viewModel, bindingContext )
		{
			var value = allBindingsAccessor().value;
			value.extend(valueAccessor());
		},
		update : function( element, valueAccessor, allBindingsAccessor, viewModel, bindingContext )
		{
			//allBindingsAccessor().value.extend(valueAccessor());
		}
	};
	
	ko.bindingHandlers.slug = {
		init : function( element, valueAccessor, allBindingsAccessor, viewModel, bindingContext )
		{
		},
		update : function( element, valueAccessor, allBindingsAccessor, viewModel, bindingContext )
		{
			var container = allBindingsAccessor().value;
			var input = container().toLowerCase();
			var replace = input.replace(/[^a-zA-Z0-9]+/g, '-');
			container( replace );
		}
	};
	
	ko.modifiable = function( model ) {
		var result = ko.observable(), state = null, watching = ko.observableArray([]), group, method;

		result.original = ko.observable();
		result.isModified = ko.observable();
		result.isValidating = ko.observable();
		result.isValid = ko.observable();
		result.hasChanges = ko.observable();
		
		function update()
		{
			var validating = isValidating( result() );
			result.isValidating( validating );
			
			var valid = !validating && !group().length;
			result.isValid( valid );

			result.hasChanges( valid && result.isModified() );
		}
		
		function changed(target, trigger)
		{
			var item = result();
			var modified = state !== ko.mapping.toJSON( item );
			result.isModified( modified );

			setTimeout( function() {
				update();
				result.isModified( modified );
			}, 0 );
		}
		
		function isValidating( item )
		{
			for ( var i in item )
			{
				var m = item[i].isValidating;
				if ( m && m() )
				{
					switch ( watching.indexOf(i) )
					{
					case -1:
						watching.push(i);
						var subscription = m.subscribe(function(value) {
							if ( !value )
							{
								update( result );
								subscription.dispose();
								watching.remove( i );
							}
						});
						break;
					}
					return true;
				}
			}
			return false;
		};
		
		function reset()
		{
			if ( result.watcher )
			{
				result.watcher.pauseWatch();
				delete result.watcher;
			}
			result.isModified( false );
			var target = result();
			result.watcher = ko.watch( target, changed );
			delete group;
			group = ko.validation.group( target );
			update();
		}
		
		result.accept = function()
		{
			var item = ko.mapping.toJS( result() );

			state = ko.toJSON( item );
			
			reset();
			return item;
		};

		result.cancel = function()
		{
			var item = ko.mapping.fromJSON( state );
			result( item );
			reset();
			return item;
		};

		result.apply = function()
		{
			if ( method && result.isValid() )
			{
				return method.apply( arguments );
			}
			return false;
		};

		result.assign = function( item, callback )
		{
			method = callback;
			
			result.original( item );

			state = ko.toJSON( item );

			var current = ko.mapping.fromJS( item );
			result( current );
		
			reset();
		};
		
		if ( model )
		{
			result.assign( model );
		}

		
		return result;
	};

	ko.bindingHandlers.locateCommand = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var settings = valueAccessor();
			var handler = settings.handler || 'click';
			settings.command = require(settings.path);
			var target = function () {
				var parameter = valueAccessor().parameter || viewModel;
				var parameters = $.makeArray(arguments);
				parameters.shift();
				parameters.unshift(parameter);
				settings.command.apply(settings.command, parameters);
			};


			if (ko.bindingHandlers[handler] !== undefined) {
				var accessor = ko.utils.wrapAccessor(target);
				ko.bindingHandlers[handler].init(element, accessor, allBindingsAccessor, viewModel);
			} else {
				var events = {};
				events[handler] = command;
				ko.bindingHandlers.event.init(element, ko.utils.wrapAccessor(target), allBindingsAccessor, viewModel);
			}
		},
		update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var settings = valueAccessor();
			settings.command = require(settings.path);
			ko.bindingHandlers[settings.interaction || "enable"].update(element, settings.command.canExecute, allBindingsAccessor, viewModel);
		}
	};

	ko.bindingHandlers.jqxWidget = {
		_refresh: function (element, valueAccessor) {
			var target = $(element);
			var p = valueAccessor();
			target[p.type](p.parameters || {});

			if (p.bind) {
				$.each(p.bind, function (key, value) {
					target.bind(key, value);
				});
			}
		},
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			instance.ensureAttached(element, function () {
				ko.bindingHandlers.jqxWidget._refresh(element, valueAccessor);
			});
		},
		update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			instance.ensureAttached(element, function () {
				ko.bindingHandlers.jqxWidget._refresh(element, valueAccessor);
			});
		}
	};
	
	ko.virtualElements.allowedBindings.jqxWidget = true;

	var monitors = {};

	var instance = {
		overrides: [],
		ensureAttached: function (element, delegate) {
			var check = function () {
				if ($(element).parents(':last').is('html')) {
					setTimeout(delegate, 100);
					return true;
				}
				return false;
			};

			if (!check()) {
				var id = setInterval(function () {
					if (check()) {
						clearInterval(id);
					}
				}, 10);
			}
		},

		toHTML: function (text) {
			var charEncodings = {
				"\t": "&nbsp;&nbsp;&nbsp;&nbsp;",
				" ": "&nbsp;",
				"&": "&amp;",
				"<": "&lt;",
				">": "&gt;",
				"\n": "<br />",
				"\r": "<br />"
			};
			var space = /[\t ]/;
			var noWidthSpace = "&#8203;";
			text = (text || "") + "";  // make sure it's a string;
			text = text.replace(/\r\n/g, "\n");  // avoid adding two <br /> tags
			var html = "";
			var lastChar = "";
			for (var i in text) {
				if (!$.isFunction(text[i])) {
					var c = text[i];
					var charCode = text.charCodeAt(i);
					if (space.test(c) && !space.test(lastChar) && space.test(text[i + 1] || "")) {
						html += noWidthSpace;
					}
					html += c in charEncodings ? charEncodings[c] :
					charCode > 127 ? "&#" + charCode + ";" : c;
					lastChar = c;
				}
			}
			return html;
		},

		sizeMonitor: function (item) {
			function create() {
				var created = {
					width: ko.observable("{0}px".format(item.width())),
					height: ko.observable("{0}px".format(item.height()))
				};
				item.bind('resize', function (e) {
					var width = item.width();
					result.width(width);
					var height = item.height();
					result.height(height);
				});
				return created;
			}
			var result = monitors[item.context] || (monitors[item.context] = create());
			return result;
		},
	};
	return instance;
});



(function (ko) {
	ko.utils.wrapAccessor = function (accessor) {
		return function () {
			return accessor;
		};
	};

	ko.command = function (options) {
		var self = function () {
			return self.execute.apply(this, arguments);
		},
		canExecuteDelegate = options.canExecute,
        executeDelegate = options.execute;

		self.canExecute = ko.computed(function () {
			return canExecuteDelegate ? canExecuteDelegate() : true;
		});

		self.execute = function (arg1, arg2) {
			return self.canExecute() ? executeDelegate.apply(this, [arg1, arg2]) : null;
		};

		return self;
	};

	ko.asyncCommand = function (options) {
		var self = function () {
			return self.execute.apply(this, arguments);
		},
        canExecuteDelegate = options.canExecute,
        executeDelegate = options.execute,
        completeCallback = function (error) {
        	self.error(error);
        	self.isExecuting(false);
        };

		self.error = ko.observable();

		self.isExecuting = ko.observable();

		self.canExecute = ko.computed(function () {
			return canExecuteDelegate ? canExecuteDelegate(self.isExecuting()) : !self.isExecuting();
		});

		self.execute = function (arg1, arg2) {
			if (self.canExecute()) {
				var args = [];
				if (executeDelegate.length >= 2) {
					args.push(arg1);
				}
				if (executeDelegate.length >= 3) {
					args.push(arg2);
				}

				args.push(completeCallback);
				self.isExecuting(true);
				return executeDelegate.apply(this, args);
			}
			return null;
		};

		return self;
	};
})(ko);