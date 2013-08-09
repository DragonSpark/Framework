define(function (require) {
	var instance = {
		route : "Not Found",
		activate: function(route) {
			instance.route = route;
		}
	};
	return instance;
});