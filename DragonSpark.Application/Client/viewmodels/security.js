define(["./navigation", "plugins/router"], function (navigation, router) {
	var model = {
		navigation: ko.observableArray([]),
	};
	
	router.on("router:route:activating").then(function (instance, instruction) {
		if (instance == model) {
			var navigationModel = instruction.config.childRouter.navigationModel();
			model.router = instruction.config.childRouter;
			model.navigation(navigationModel);
		}
	});

	return model;
});