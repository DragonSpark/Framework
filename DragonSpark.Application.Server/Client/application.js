requirejs.config({
	paths: {
		text: "DragonSpark.Client/Core/text",
		durandal: "DragonSpark.Client/Durandal",
		dragonspark: "DragonSpark.Client",
		
		plugins: "DragonSpark.Client/plugins",
		transitions: "DragonSpark.Client/transitions",
		widgets: "DragonSpark.Client/widgets",
		
		application: "DragonSpark.Application.Client",
		viewmodels: "DragonSpark.Application.Client/viewmodels",
		views: "DragonSpark.Application.Client/views"
	}
});

define( "jquery", [], function () { return jQuery; } );
define( "knockout", [], function () { return ko; } );

define(["dragonspark/application"], function (dragonSpark) {
	dragonSpark.initialize();
});