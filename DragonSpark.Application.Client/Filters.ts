/// 
/// Filters
/// -------------------------------------------------------------------------------------------------------------------
/// <reference path="_references.ts" />

angular.module( "app.filters", [] )
	.filter( "interpolate", [<any> "version", version=> text=> String( text ).replace( /\%VERSION\%/mg, version )] );