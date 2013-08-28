define(["dragonspark/configuration", "./navigation", "plugins/router"], function (configuration, navigation, router) {
	var model = {
		navigation: ko.observableArray([]),
		initialize: function()
		{
			router.on("router:route:activating").then( model.refresh );
			return model;
		},
		refresh: function( instance, instruction )
		{
			if (instance == model)
			{
				var navigationModel = instruction.config.childRouter.navigationModel();
				model.router = instruction.config.childRouter;
				model.navigation(navigationModel);
			}
		}
	};
	
	configuration.on("application:configuration:refreshed", function() {
		model.initialize();
		// var activeInstruction = router.activeInstruction();
		// model.refresh( model, activeInstruction );
	} );
	
	return model.initialize();
});