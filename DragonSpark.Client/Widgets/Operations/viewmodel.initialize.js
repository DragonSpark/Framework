﻿define(["./monitor", "dragonspark/application"], function ( monitor, application ) {
	ko.bindingHandlers.operation = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var settings = valueAccessor();
			var operation = monitor.createOperation(bindingContext, settings);
			
			if ( settings.event )
			{
				var source = settings.eventSource || viewModel;
				source.off( settings.event, null, ko.bindingHandlers.operation );
				source.on( settings.event, operation, ko.bindingHandlers.operation );
			}
			else
			{
				var handler = settings.handler || 'click';
				if (ko.bindingHandlers[handler] !== undefined) {
					var accessor = ko.utils.wrapAccessor(operation);
					ko.bindingHandlers[handler].init(element, accessor, allBindingsAccessor, viewModel);
				} else {
					var events = {};
					events[handler] = operation;
					ko.bindingHandlers.event.init(element, ko.utils.wrapAccessor(events), allBindingsAccessor, viewModel);
				}
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
	});
	return {};
});