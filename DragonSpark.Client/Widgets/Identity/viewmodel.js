define(["dragonspark/configuration", "./core"], function ( configuration, core ) {
	var ctor = function() {
		this.configuration = configuration;
		this.displayName = ko.computed(function() {
			var profile = configuration().UserProfile;
			var result = profile ? profile.DisplayName || profile.Name : "Guest";
			return result;
		});
		this.provider = ko.computed(function () {
			var profile = configuration().UserProfile;
			var result = core.claim( profile, "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider" );
			return result;
		});
	};

	return ctor;
});