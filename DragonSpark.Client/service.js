define(["durandal/system"], function (system) {

	function determineParameter(parameter) {
		try {
			return ko.toJSON(parameter);
		} catch(e) {
			return null;
		} 
	}

	function invoke(name, parameter, method, controller) {
		var result = system.defer(function (dfd) {
			var serialize = parameter ? determineParameter(parameter) : null;
			$.ajax({
				url: "{0}/{1}".format(controller, name),
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
		invoke: function (method, parameter, controller) {
			return invoke(method, parameter, "POST", controller || "Session");
		},
		call: function (method, parameter, controller) {
			return invoke(method, parameter, null, controller || "Session");
		}
	};
});