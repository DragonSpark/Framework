///
/// Directives
/// -------------------------------------------------------------------------------------------------------------------
angular.module("app.directives", []).directive("appASDF", [
    "version",
    function (version) {
        return function (scope, elm, attrs) {
            elm.text(version);
        };
    }
]);
//# sourceMappingURL=Directives.js.map
