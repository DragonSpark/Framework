define(function (require) {
	var app = require("durandal/app"),
		instance = {};
	
	app.on("application:operations:initialize").then(function (profile) {
	}, instance);
	return instance;
});