define( [ "knockout", "plugins/serializer" ], function( ko, serializer ) {
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

	ko.bindingHandlers.initialize = 
	{
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var method = valueAccessor();
			method();
		},
		update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) { }
	};	
	
	ko.bindingHandlers.validation = 
	{
		init : function( element, valueAccessor, allBindingsAccessor, viewModel, bindingContext )
		{
			var value = allBindingsAccessor().value;
			value.extend(valueAccessor());
		},
		
		update : function( element, valueAccessor, allBindingsAccessor, viewModel, bindingContext )
		{}
	};
	
	ko.bindingHandlers.slug = 
	{
		init : function( element, valueAccessor, allBindingsAccessor, viewModel, bindingContext )
		{},
		
		update : function( element, valueAccessor, allBindingsAccessor, viewModel, bindingContext )
		{
			var container = allBindingsAccessor().value;
			var input = container().toLowerCase();
			var replace = input.replace(/[^a-zA-Z0-9]+/g, '-');
			container( replace );
		}
	};
	
	ko.bindingHandlers.locateCommand = 
	{
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

	ko.tracker = function( item, modified ) {
		var result		= ko.observable(),
			initial		= ko.observable( serializer.serialize( item ) ),
			isModified	= ko.observable( modified );

		result.isModified = ko.computed( function() {
			return isModified() || initial() !== serializer.serialize( item );
		} );

		result.reset = function( i, clear )
		{
			var value = ( !i && clear ) ? null : i || ( initial() ? serializer.deserialize( initial() ) : null );
			initial( serializer.serialize( value ) );
			isModified( false );
			result( value );
		};

		return result;
	};
	return {};
} );