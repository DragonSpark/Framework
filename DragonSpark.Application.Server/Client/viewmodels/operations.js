define(["durandal/system"], function (system) {
	var instance = {
		immediate: function () {
			$ds.log("Executing immediate operation!");
		},
		operation: function () {
			$ds.log("Starting operation...");
			var result = system.defer(function (dfd) {
				$ds.wait(2000).then(function () {
					$ds.log("Operation completed!");
					dfd.resolve();
				});
			}).promise();
			return result;
		},

		throwException: function () {
			throw new Error('Hello World');
		}
		
	};
	return instance;
});