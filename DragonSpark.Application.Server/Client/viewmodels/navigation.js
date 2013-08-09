define(["plugins/router", "dragonspark/configuration"], function (router, configuration) {
	var instance = {
		router: router,
		default: function () {
			var result = configuration.default();
			return result;
		}
	};
	return instance;
});