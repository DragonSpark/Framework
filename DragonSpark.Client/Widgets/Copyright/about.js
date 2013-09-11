define( [ "plugins/dialog", "dragonspark/view" ], function ( dialog, view ) {
	var ctor = function (details, user) {
		this.details = details;
		this.user = user;
	};
	ctor.prototype.close = function() {
		dialog.close(this);
	};

	ctor.prototype.getView = function()
	{
		return view.overrides[ "widgets.copyright.about.view" ];
	};
	return ctor;
});