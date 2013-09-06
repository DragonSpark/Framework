define(["durandal/app", "durandal/events", "durandal/viewLocator", "plugins/router", "plugins/history", "plugins/widget", "durandal/system", "application.service"], function (app, events, viewLocator, router, history, widget, system, service) {
	var instance = $.extend( ko.observable(), {
		_build : function (targetRouter, items) 
		{
			targetRouter.map( items ).buildNavigationModel();

			var moduleId = instance().Navigation.NotFound.moduleId;
			targetRouter.mapUnknownRoutes( moduleId );
		},
		
		_determineActiveRouter : function( current )
		{
			var parent = current || router;
			var activeItem = parent.activeItem();
			var item = activeItem ? activeItem.router : null;
			var result = item ? instance._determineActiveRouter( item ) : parent;
			return result;
		},
		
		activateRouter : function( module )
		{
			if ( !module.router )
			{
				var active = instance._determineActiveRouter();
				var config = active.activeInstruction().config;
				var id = config.moduleId;

				var options = { moduleId: id, fromParent: true };
				module.router = active.createChildRouter().makeRelative( options );
				instance._build( module.router, config.children );

				/*var navigation = module.navigationModel || ( module.navigationModel = ko.observableArray( [] ) );
				navigation( module.router.navigationModel() );*/

				var subscribe = function()
				{
					module.router.on( "router:route:before-config" ).then( function( configuration ) {
						var moduleId = instance().Navigation.NotFound.moduleId;
						if ( configuration.moduleId == id + "/" + moduleId )
						{
							configuration.moduleId = moduleId;
						}
					} );

					module.router.on( "router:navigation:attached", function( currentActivation, currentInstruction, child ) {
						if ( module.__resetRouter__ )
						{
							module.__resetRouter__ = false;
							module.router.reset();
							module.router.makeRelative( options );
							var item = child.parent.activeInstruction().config;
							instance._build( module.router, item.children );
							subscribe();
						}
					} );
				};
				subscribe();

				

				instance.on( "application:configuration:refreshed", function() {
					// var activeRouter = instance._determineActiveRouter();
					// var isactive = activeRouter == module.router;
					module.__resetRouter__ = true;
					// instance._registerChildRouter( module );
					/*if ( isactive )
					{
						module.router.attached();
					}*/
				} );
			}

			return module.router;
		},
		
		_assign: function (data) {
			instance(data);
			
			router.deactivate();
			router.reset();
			with (data) {
				instance._build( router, Navigation.Routes );
				// TODO: This is dumb.
				ApplicationDetails.Version = new Version(ApplicationDetails.Version);
				ApplicationDetails.DeploymentDate = new Date(ApplicationDetails.DeploymentDate);
			}
		},
		initialize: function()
		{
			return service.getConfiguration().then(function (data) {
				return system.defer(function (dfd) {
					instance._assign(data);
				
					with (instance()) {
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
		
		default: function () {
			var result = Enumerable.From(router.routes).FirstOrDefault();
			return result;
		},
		
		refresh : function()
		{
			var result = service.getConfiguration()
				.then(function (data) {
					instance.trigger("application:configuration:refreshing", instance, data );
					instance._assign(data);
					instance.trigger("application:configuration:refreshed", instance, data );
				});
			return result;
		}
	});

	/*router.on("router:navigation:attached", function (routeInfo, params, module) {
		routers.map(function (i) {
			if (i.parent == router) {
				i.attached();
			}
		});
	});*/

	router.on("router:navigation:complete", function (routeInfo, params, module) {
		var title = instance().ApplicationDetails.Title;
		document.title = "{0} - {1}".format(params.config.title || "Not Found", title);
	});
	events.includeIn(instance);
	return instance;
});