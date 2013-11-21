/// 
/// AngularJS Modules
/// -------------------------------------------------------------------------------------------------------------------



// Declare app level module which depends on filters, and services
angular.module( "Application", [ "ngRoute", "app.filters", "app.services", "app.directives", "app.controllers"])
	.config([<any> "$locationProvider", "$routeProvider", ($locationProvider: ng.ILocationProvider, $routeProvider: ng.route.IRouteProvider) => {
	$locationProvider.html5Mode(true);
	$routeProvider
		.when("/view1", { templateUrl: "/-=clientResource=-/DragonSpark.Application.Client;component/Views/View1.html", controller: "FirstCtrl" })
		.when("/view2", { templateUrl: "/-=clientResource=-/DragonSpark.Application.Client;component/Views/View2.html", controller: "SecondCtrl" })
		.otherwise( { redirectTo: "/view1" } ); 
	}]);