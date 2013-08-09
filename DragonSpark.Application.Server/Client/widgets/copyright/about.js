define(["plugins/dialog"], function (dialog) {
	var ctor = function (details, user) {
		this.details = details;
		this.user = user;
	};
	ctor.prototype.close = function() {
		dialog.close(this);
	};
	return ctor;
});