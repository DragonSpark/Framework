/// <reference path="Resources/Scripts/typings/_references.ts" />

window.onload = ()=>
{
	window.alert("Hello World!");
	angular.module("app.controllers", [])
		.controller( "FirstCtrl", [<any> "$scope", $scope => {

		} ] )
		.controller( "SecondCtrl", [<any> "$scope", $scope => {

		} ] );
};
