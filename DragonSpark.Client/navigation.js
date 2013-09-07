define(["plugins/router", "./configuration"], function ( router, configuration ) {
	function determineActiveRouter( current )
	{
		var parent = current || router;
		var activeItem = parent.activeItem();
		var item = activeItem ? activeItem.router : null;
		var result = item ? determineActiveRouter( item ) : parent;
		return result;
	}
	
	function build( target, items ) 
	{
		var result = target.map( items ).buildNavigationModel();
		target.mapUnknownRoutes( instance.data.NotFound.moduleId );
		return result;
	}

	var instance =
	{
		load : function( routes )
		{
			router.deactivate();
			router.reset();

			build( router, routes );
		},

		data : null,
		
		refresh : function()
		{
			var fragment = router.activeInstruction().fragment;
			router.loadUrl( fragment );
		},

		activateRouter : function( module )
		{
			if ( !module.router )
			{
				var active = determineActiveRouter();
				var config = active.activeInstruction().config;
				var id = config.moduleId;

				var options = { moduleId : id, fromParent : true };
				module.router = active.createChildRouter().makeRelative( options );
				build( module.router, config.children );

				var subscribe = function()
				{
					module.router.on( "router:route:before-config" ).then( function( configuration ) {
						var moduleId = instance.data.NotFound.moduleId;
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
							build( module.router, item.children );
							subscribe();
						}
					} );
				};
				subscribe();

				configuration.on( "application:configuration:refreshed", function() {
					module.__resetRouter__ = true;
				} );
			}

			return module.router;
		}
	};

	configuration.on( "application:configuration:refreshing", function( module, data ) {
		instance.data = data.Navigation;
		instance.load( data.Navigation.Routes );
	} );

	router.on("router:navigation:complete", function (routeInfo, params, module) {
		var title = configuration().ApplicationDetails.Title;
		document.title = "{0} - {1}".format(params.config.title || "Not Found", title);
	});
	return instance;
});