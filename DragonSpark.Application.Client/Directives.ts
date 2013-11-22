/// 
/// Directives
/// -------------------------------------------------------------------------------------------------------------------

angular.module( "app.directives", [] )
	.directive("appASDF", [<any> "version", version => (scope, elm, attrs) =>
	{
		elm.text(version);
	}]);