define(["dragonspark/enum", "./monitor", "dragonspark/application"], function ( enumerable, monitor, application ) {
	var instance = {
		ExecutionStatus: new enumerable({
			None: {},
			Executing: {},
			Canceling: {},
			Canceled: { description: "Canceled by User" },
			Aborted: {},
			Completed: {},
			CompletedWithException: { description: "Completed with Exception" },
			CompletedWithFatalException: { description: "Completed with Fatal Exception (oops!)" }
		}),
		ExceptionHandlingAction: new enumerable({
			None: {},
			Continue: {},
			Throw: {}
		}),
		_initialized : false,
		initialize: function()
		{
			if ( !instance._initialized && ( instance._initialized = true ) )
			{
				ko.bindingHandlers.operation = {
					init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
						var settings = valueAccessor();
						var operation = monitor.createOperation(bindingContext, settings);
						var handler = settings.handler || 'click';
			
						if (ko.bindingHandlers[handler] !== undefined) {
							var accessor = ko.utils.wrapAccessor(operation);
							ko.bindingHandlers[handler].init(element, accessor, allBindingsAccessor, viewModel);
						} else {
							var events = {};
							events[handler] = operation;
							ko.bindingHandlers.event.init(element, ko.utils.wrapAccessor(events), allBindingsAccessor, viewModel);
						}
					},
					update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
						var commands = valueAccessor();
						var canExecute = ko.bindingHandlers.operation._determineCanExecute( commands );
						if (canExecute) {
							ko.bindingHandlers.enable.update(element, canExecute, allBindingsAccessor, viewModel);
						}
					},
		
					_determineCanExecute: function (commands) {
						for (var command in commands) {
							if (commands[command] && commands[command].canExecute) {
								return  commands[command].canExecute;
							}
						}
						return commands.canExecute;
					}
				};
	
				application.on("application:refresh").then(function () {
					monitor._broadcast("Refreshing Application... Please Wait.", "application:operations:refreshing", "application:operations:refreshed");
				}, instance);
			}
			return instance;
		}
	};
	
	return instance;
});