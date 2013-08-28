define(["durandal/app", "durandal/events", "durandal/viewLocator", "plugins/router", "plugins/history", "plugins/widget", "durandal/system", "dragonspark/service"], function (app, events, viewLocator, router, history, widget, system, service) {
	function determineRoute(route) {
		var colonIndex = route.indexOf(":");
		var length = colonIndex > 0 ? colonIndex - 1 : route.length;
		var splatIndex = route.indexOf("*");

		var index = splatIndex == -1 ? length : splatIndex;

		var result = route.substring(0, index);
		return result;
	}
	var routers = [];
	
	var instance = $.extend(ko.observable(null), {
		_build: function (targetRouter, items) {
			routers.push(targetRouter);
			targetRouter.map( items ).buildNavigationModel();
			
			var moduleId = instance().Navigation.NotFound.moduleId;
			targetRouter.mapUnknownRoutes(moduleId);

			items.map(function (i) {
				if (i.children.length) {
					var config = {
						moduleId: i.moduleId,
						route: determineRoute(i.route)
					};
					i.childRouter = targetRouter.createChildRouter().makeRelative(config);
					i.childRouter.on( 'router:route:before-config' ).then( function( configuration ) {
						if ( configuration.moduleId == i.moduleId + "/" + moduleId )
						{
							configuration.moduleId = moduleId;	
						}
					} );
					instance._build(i.childRouter, i.children);
				}
			});
		},

		_assign: function (data) {
			instance(data);
			
			routers.map(function (i) {
				i.reset();
			});
			routers.length = 0;
			with (data) {
				instance._build(router, Navigation.Routes);

				// TODO: This is dumb.
				ApplicationDetails.Version = new Version(ApplicationDetails.Version);
				ApplicationDetails.DeploymentDate = new Date(ApplicationDetails.DeploymentDate);
			}
		},
		initialize: function() {
			return service.call( "Configuration" ).then(function (data) {
				return system.defer(function (dfd) {
					instance._assign(data);
				
					with (instance()) {
						system.debug(EnableDebugging);

						var initializers = Enumerable.From(Initializers),
							commands = Enumerable.From(Commands),
							widgets = Enumerable.From(Widgets);

						var modules = initializers.Concat(commands).Concat(widgets).Where("$.Path != null").Select("$.Path").ToArray();

						require(modules, function () {
							app.title = ApplicationDetails.Title;
							var names = widgets.Select(function (x) { return x.Name; }).ToArray();
							
							app.configurePlugins({ router: true, dialog: true, widget: { kinds: names }});

							app.start().then(function () {
								viewLocator.useConvention();

								var shell = Navigation.Shell;
								app.setRoot(shell.moduleId, shell.transitionName);
								router.mapUnknownRoutes(Navigation.NotFound.moduleId);
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
		
		refresh : function() {
			return service.call( "Configuration" ).then(function (data) {
				instance.trigger("application:configuration:refreshing");
				instance._assign(data);
				
				instance.trigger("application:configuration:refreshed");
			});
		}
	});

	router.on("router:navigation:attached", function (routeInfo, params, module) {
		routers.map(function (i) {
			if (i.parent == router) {
				i.attached();
			}
		});
	});

	router.on("router:navigation:complete", function (routeInfo, params, module) {
		var title = instance().ApplicationDetails.Title;
		document.title = "{0} - {1}".format(params.config.title || "Not Found", title);
	});
	events.includeIn(instance);
	return instance;
});