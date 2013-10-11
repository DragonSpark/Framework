define(["durandal/system", "dragonspark/configuration", "dragonspark/application"], function (system, configuration, application) {
	function initialize(widget, model)
	{
		$(model).data(key, widget);
		if (model.__moduleId__ == configuration().Navigation.Shell.moduleId) 
		{
			switch ( $.inArray(model, initialized) )
			{
			case -1:
				initialized.push( model );
				broadcast("Initializing Application... Please Wait.", "application:operations:initializing", "application:operations:initialized").then(application.activate);
				break;
			}
		}
	}
	
	function broadcast(title, eventName, completedName) {
		var apply = function(m) {
			var profile = {
				title: title,
				allowCancel: false,
				closeOnCompletion: true,
				anyExceptionIsFatal: true,
				items: ko.observableArray([])
			};
			application.trigger(eventName, profile);
			var always = m.monitor(profile).fin(function () {
				application.trigger(completedName, profile);
			});
			return always;
		};

		var result = system.defer(function (dfd) {
			system.acquire( configuration().Navigation.Shell.moduleId ).then(function (shell) {
				var attach = function() {
					var monitor = instance.getMonitor(shell);
					apply(monitor).then(function() {
						dfd.resolve();
					});
				};

				if (shell.isAttached()) {
					attach();
				} else {
					var attached = shell.attached;
					shell.attached = $ds.chain(attached, function () { attach(); shell.attached = attached; });
				}
			});
		}).promise();
		return result;
	}

	var key = "{40125554-7683-4927-BE6A-71886DEA073D}",
	    initialized = [],
		instance = {
			_broadcast : broadcast,
			initialize: function (widget, model)
			{
				var current = $(model).data(key);
				if ( widget != current )
				{
					var apply = function() { initialize( widget, model ); };
					var result = current && current._active ? current._active.fin( apply ) : null;
					if ( !result )
					{
						apply();
					}
					return result; 
				}
				return null;
			},
			
			createOperation: function(bindingContext, settings) {
				var result = ko.asyncCommand({
					execute: function(complete) {
						var monitor = instance.getMonitor(settings.dataContext) || instance.getMonitor(bindingContext.$data) || instance.getMonitor(bindingContext.$parentContext.$data) || instance.getMonitor(bindingContext.$root);
						if ( monitor )
						{
							monitor.monitor(settings).finally(complete);
						} 
						else
						{
							system.log( "Monitor not found" );
							complete();
						}
						return settings.handler == "click";
					},

					canExecute: function(isExecuting) {
						return !isExecuting;
					}
				});
				return result;
			},
			getMonitor: function(model) {
				var result = $(model).data(key);
				return result;
			}
		};
	
	return instance;
});