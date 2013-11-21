/// 
/// Directives
/// -------------------------------------------------------------------------------------------------------------------
/// <reference path="_references.ts" />

angular.module( "app.directives", [] )
	.directive("appASDF", [<any> "version", version => (scope, elm, attrs) =>
	{
		elm.text(version);
	}]);