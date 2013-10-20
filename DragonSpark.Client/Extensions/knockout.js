define( [ "knockout", "plugins/serializer" ], function( ko, serializer ) {
	ko.observableArray.fn.pushAll = function (valuesToPush) {
		var underlyingArray = this();
		this.valueWillMutate();
		ko.utils.arrayPushAll(underlyingArray, valuesToPush);
		this.valueHasMutated();
		return this;  //optional
	};
	
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
			if (self.canExecute())
			{
				self.error();
				var args = [];
				if (executeDelegate.length >= 2) {
					args.push(arg1);
				}
				if (executeDelegate.length >= 3) {
					args.push(arg2);
				}

				args.push(completeCallback);
				self.isExecuting(true);
				var result = executeDelegate.apply( this, args );

				switch ( result && result.toString() )
				{
				case "[object Promise]":
					Q( result ).then( completeCallback, completeCallback );
					break;
				}
				return result;
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

	ko.validationMonitor = function( item )
	{
		var watch = ko.observable( item );
		var result = ko.computed( function() {
			var target = watch();
			if ( target )
			{
				for ( var i in target )
				{
					if ( target[ i ].isValid && !target[ i ].isValid() )
					{
						return false;
					}
				}
			}
			return true;
		} );
		result.monitor = function( input )
		{
			watch( input );
		};
		return result;
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
	
	// Observer plugin for Knockout http://knockoutjs.com/
	// (c) Ziad Jeeroburkhan
	// License: MIT (http://www.opensource.org/licenses/mit-license.php)
	// Version 1.1.0 beta

	ko['watch'] = function (target, options, callback) {
		/// <summary>
		///     React to changes in a specific target object or function.
		/// </summary>
		/// <param name="target">
		///     The target subscribable or object/function containing subscribables.
		///     Note that the value false can be used to prevent a subscribable from being watched.
		/// </param>
		/// <param name="options" type="object">
		///     { recurse: 1 } -> Listen to 1st level subscribables only(default).
		///     { recurse: 2 } -> Listen to nested subscribables down to the 2nd level.
		///     { recurse: true } -> Listen to all nested subscribables.
		///     { exclude: [...] } -> Property or array of properties to be ignored.
		///     This parameter is optional.
		/// </param>
		/// <param name="callback" type="function">
		///     The callback function called when changes occur.
		/// </param>
		return ko.observable().watch(target, options, callback);
	}
    
	ko.subscribable.fn['watch'] = function (target, options, valueEvaluatorFunction) {
		/// <summary>
		///     React to changes in a specific target object or function.
		/// </summary>
		/// <param name="target">
		///     The target subscribable or object/function containing the subscribable(s).
		///     Note that the value false can be used to prevent a subscribable from being watched.
		/// </param>
		/// <param name="options" type="object">
		///     { recurse: 1 } -> Listen to 1st level subscribables only(default).
		///     { recurse: 2 } -> Listen to nested subscribables down to the 2nd level.
		///     { recurse: true } -> Listen to all nested subscribables.
		///     { exclude: [...] } -> Property or array of properties to be ignored.
		///     This parameter is optional.
		/// </param>
		/// <param name="valueEvaluatorFunction" type="function">
		///     The evaluator or function used to update the value of the subscribable during changes.
		/// </param>

		// Setting the target as false prevents it from being watched later on.
		if (target === false || target === true)
			target.watchable = target;

		if (target.watchable === false)
			return;

		if (valueEvaluatorFunction == undefined) {
			valueEvaluatorFunction = options;
			options = {};
		}

		this.isPaused = false;
		var context = this;

		function watchChildren(targ, recurse) {
			if (recurse === true || recurse >= 0) {

				if (options.exclude && typeof options.exclude === 'object'
					? ko.utils.arrayIndexOf(options.exclude, targ) >= 0
					: options.exclude === targ)
					return;

				if (ko.isSubscribable(targ)) {
					if (targ() instanceof Array) {
						var previousValue;
						targ.subscribe(function (e) { previousValue = e.slice(0); }, undefined, 'beforeChange');
						targ.subscribe(function (e) {
							var editScript = ko.utils.compareArrays(previousValue, e);
							ko.utils.arrayForEach(editScript, function () {
								switch (this.status) {
									case 'deleted':
										// TODO: Deleted items need to be unsubscribed here if KnockoutJS doesn't already do it under the hood.
										valueEvaluatorFunction.call(context, target, targ, 'deleted');
										break;
									case 'added':
										watchChildren(this.value, recurse === true || Number(recurse) -1);
										valueEvaluatorFunction.call(context, target, targ, 'added');
										break;
								}
							});
							previousValue = undefined;
						});

						watchChildren(targ(), recurse === true || Number(recurse) - 1);

					} else {
						targ.subscribe(function (targetValue) {
							if (!context.isPaused) {
								var returnValue = valueEvaluatorFunction.call(context, target, targ);
								if (returnValue !== undefined)
									context(returnValue);
							}
						});
					}

				} else if (typeof targ === 'object') {
					for (var property in targ)
						watchChildren(targ[property], recurse === true || Number(recurse) - 1);

				} else if (targ instanceof Array) {
					for (var i = 0; i < targ.length; i++)
						watchChildren(targ[i], recurse === true || Number(recurse) - 1);

				} else {
					ko.computed(function () {
						var targetValue = targ();
						if (!context.isPaused) {
							// Bypass change detection for valueEvaluatorFunction during its evaluation.
							setTimeout(function () {
								returnValue = valueEvaluatorFunction.call(context, target, targ);
								// Check that a return value exists.
								if (returnValue !== undefined && returnValue !== context())
									context(returnValue);
							}, 0);
						}
					});
				}
			}
		}

		watchChildren(target, options.recurse || 1);

		this.pauseWatch = function () {
			context.isPaused = true;
		}
		this.resumeWatch = function () {
			context.isPaused = false;
		}

		return this;
	}

	return {};
} );