define(["durandal/system"], function (system) {
	var instance = {
		immediate: function () {
			system.log("Executing immediate operation!");
		},
		operation: function () {
			system.log("Starting operation...");
			var result = system.defer(function (dfd) {
				system.wait(2000).then(function () {
					system.log("Operation completed!");
					dfd.resolve();
				});
			}).promise();
			return result;
		},

		throwException: function () {
			throw new Error("Hello World");
		}
		
	};
	return instance;
});