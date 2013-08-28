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

define( "service", [], function () { return $.connection.application.server; } );

define(["dragonspark/application"], function (dragonSpark) {
	$.connection.application.client.exceptionHandler = function( error ) { $ds.lastServiceError = error; };
	dragonSpark.initialize();
});