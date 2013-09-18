define(["durandal/app", "durandal/events", "durandal/viewLocator", "plugins/widget", "durandal/system", "application.service"], function (app, events, viewLocator, widget, system, service) {
	var instance = $.extend( ko.observable(), 
		{
			_refresh: function ( data ) {
				instance.trigger("application:configuration:refreshing", instance, data );
				instance( data );
			
				with (data) 
				{
					// TODO: This is dumb.
					ApplicationDetails.Version = new Version(ApplicationDetails.Version);
					ApplicationDetails.DeploymentDate = new Date(ApplicationDetails.DeploymentDate);
				}
				instance.trigger("application:configuration:refreshed", instance, data );
			},
		
		initialize: function()
		{
			return service.getConfiguration().then(function (data) {
				return system.defer(function (dfd) {
					instance._refresh( data );
				
					with ( instance() ) {
						system.debug(EnableDebugging);

						var initializers = Enumerable.From(Initializers),
							commands = Enumerable.From(Commands),
							widgets = Enumerable.From(Widgets);

						widgets.Where( "$.ViewId != null" ).ToArray().map( function( i ) {
							widget.mapKind( i.Name, i.ViewId );
						} );

						var modules = widgets.Concat(commands).Concat(initializers).Where("$.Path != null").Select("$.Path").ToArray();

						require(modules, function () {
							app.title = ApplicationDetails.Title;
							var names = widgets.Select(function (x) { return x.Name; }).ToArray();
							
							app.configurePlugins({ router: true, dialog: true, widget: { kinds: names }});
							app.start().then(function () {
								viewLocator.useConvention();

								var shell = Navigation.Shell;
								app.setRoot(shell.moduleId, shell.transitionName);
								dfd.resolve();
							});
						});
					}
				});
			});
		},
		
		refresh : function()
		{
			var result = service.getConfiguration().then(function (data) { instance._refresh(data); });
			return result;
		}
	});

	events.includeIn(instance);
	return instance;
});