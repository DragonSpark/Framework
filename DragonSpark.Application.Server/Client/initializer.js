requirejs.config({
	baseUrl: "/Client",
	paths: {
		text: "DragonSpark.Client/Core/text",
		durandal: "DragonSpark.Client/Durandal",
		dragonspark: "DragonSpark.Client",
		
		plugins: "DragonSpark.Client/Plugins",
		transitions: "DragonSpark.Client/Transitions",
		widgets: "DragonSpark.Client/Widgets",
		
		application: "DragonSpark.Application.Client",
		viewmodels: "DragonSpark.Application.Client/ViewModels",
		views: "DragonSpark.Application.Client/Views"
	}
});

define( "jquery", [], function () { return jQuery; } );
define( "knockout", [], function () { return ko; } );
define( "breeze", [], function () { return breeze; } );

define( ["dragonspark/application"], function (dragonSpark) {
	dragonSpark.initialize();
});