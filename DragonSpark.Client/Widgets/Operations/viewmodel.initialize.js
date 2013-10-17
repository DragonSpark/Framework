define(["./monitor", "dragonspark/application"], function ( monitor, application ) {
	function canExecute(commands) {
		for (var command in commands) {
			if (commands[command] && commands[command].canExecute) {
				return  commands[command].canExecute;
			}
		}
		return commands.canExecute;
	}

	ko.bindingHandlers.operation = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var settings = valueAccessor();
			var operation = monitor.createOperation(bindingContext, settings);

			var register = function( handler )
			{
				if ( ko.bindingHandlers[ handler ] !== undefined )
				{
					var accessor = ko.utils.wrapAccessor( operation );
					ko.bindingHandlers[ handler ].init( element, accessor, allBindingsAccessor, viewModel );
				} else
				{
					var events = {};
					events[ handler ] = operation;
					ko.bindingHandlers.event.init( element, ko.utils.wrapAccessor( events ), allBindingsAccessor, viewModel );
				}
			};

			if ( settings.event )
			{
				var source = settings.eventSource || viewModel;
				source.off( settings.event, null, ko.bindingHandlers.operation );
				source.on( settings.event, operation, ko.bindingHandlers.operation );
				if ( settings.handler )
				{
					register( settings.handler );
				}
			}
			else
			{
				register( settings.handler || 'click' );
			}
		},
		update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var commands = valueAccessor();
			var execute = canExecute( commands );
			if (execute) {
				ko.bindingHandlers.enable.update(element, execute, allBindingsAccessor, viewModel);
			}
		}
	};
	
	application.on("application:refresh").then(function () {
		monitor._broadcast("Refreshing Application... Please Wait.", "application:operations:refreshing", "application:operations:refreshed");
	});
	return {};
});