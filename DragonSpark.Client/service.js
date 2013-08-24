define(["durandal/system", "plugins/serializer"], function (system, serializer) {

	function determineParameter(parameter) {
		try {
			return ko.toJSON(parameter);
		} catch(e) {
			return null;
		} 
	}

	function invoke(name, parameter, method) {
		var result = system.defer(function (dfd) {
			var serialize = determineParameter(parameter);
			$.ajax({
				url: "Client/{0}".format(name),
				type: method ? method : parameter ? "POST" : "GET",
				dataType: "json",
				data: serialize,
				contentType: 'application/json; charset=utf-8',
				traditional: true,
				success: function (data) {
					dfd.resolve(data);
				},
				error: function (x, y, z) {
					var e = new ServiceError(x.responseText);
					dfd.reject(e);
				}
			});
		});
		return result;
	}

	return {
		invoke: function (method, parameter) {
			return invoke(method, parameter, "POST");
		},
		call: function (method, parameter) {
			return invoke(method, parameter);
		}
	};
});

function ServiceError(message) {
	this.message = message;
	this.stack = "None Available.";
}
ServiceError.prototype = new Error;