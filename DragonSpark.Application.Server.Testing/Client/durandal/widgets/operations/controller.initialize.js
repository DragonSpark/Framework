define(function (require) {
	var enumerable = require("durandal/plugins/enum");
	var instance = {
		Key: "{40125554-7683-4927-BE6A-71886DEA073D}",
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
		determineOperationMonitor: function (element) {
			var data = ko.dataFor(element);
			var result = $(data).data(instance.Key);
			return result;
		}
	};
	
	ko.bindingHandlers.operation = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var settings = valueAccessor();
			var operation = ko.bindingHandlers.operation._createOperation(element, settings);
			var handler = settings.handler || 'click';
			var paths = Enumerable.From(settings.items).Where("$.path").Select("$.path").ToArray();
			require(paths);

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
		_createOperation: function (element, settings) {
			var result = ko.asyncCommand({
				execute: function (complete) {
					var monitor = instance.determineOperationMonitor(element);
					monitor.monitor(settings).always(complete);
				},

				canExecute: function (isExecuting) {
					return !isExecuting;
				}
			});
			return result;
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
	
	return instance;
});