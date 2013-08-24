define(["durandal/system", "dragonspark/configuration", "dragonspark/application"], function (system, configuration, application) {
	var key = "{40125554-7683-4927-BE6A-71886DEA073D}",
	    initialized = [],
		instance = {
			initialize: function (widget, model)
			{
				var current = $(model).data(key);
				if ( widget != current )
				{
					var apply = function() { instance._apply( widget, model ); };
					var result = current && current._active ? current._active.always( apply ) : null;
					if ( !result )
					{
						apply();
					}
					return result;
				}
				return null;
			},
			
			_apply : function(widget, model)
			{
				$(model).data(key, widget);
				if (model.__moduleId__ == configuration().Navigation.Shell.moduleId) 
				{
					switch ( $.inArray(model, initialized) )
					{
					case -1:
						initialized.push( model );
						instance._broadcast("Initializing Application... Please Wait.", "application:operations:initializing", "application:operations:initialized").then(function () {
							application.activate();
						});
						break;
					}
				}
			},
			
			createOperation: function(bindingContext, settings) {
				var result = ko.asyncCommand({
					execute: function(complete) {
						var monitor = instance.getMonitor(settings.dataContext) || instance.getMonitor(bindingContext.$data) || instance.getMonitor(bindingContext.$root);
						monitor.monitor(settings).always(complete);
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
			},
		
			_broadcast: function (title, eventName, completedName) {
				var apply = function(m) {
					var profile = {
						title: title,
						allowCancel: false,
						closeOnCompletion: true,
						anyExceptionIsFatal: true,
						items: ko.observableArray([])
					};
					application.trigger(eventName, profile);
					var always = m.monitor(profile).always(function () {
						application.trigger(completedName, profile);
					});
					return always;
				};

				var result = system.defer(function (dfd) {
					require([configuration().Navigation.Shell.moduleId], function (shell) {
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
		};
	
	application.on("application:refresh").then(function () {
		instance._broadcast("Refreshing Application... Please Wait.", "application:operations:refreshing", "application:operations:refreshed");
	}, instance);

	return instance;
});