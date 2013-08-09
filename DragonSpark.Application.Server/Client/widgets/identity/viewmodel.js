define(["dragonspark/configuration"], function ( configuration ) {
	var ctor = function() {
		this.configuration = configuration;
		this.displayName = ko.computed(function() {
			var profile = configuration().UserProfile;
			var result = profile ? profile.DisplayName || profile.Name : "Guest";
			return result;
		});
		this.provider = ko.computed(function () {
			var profile = configuration().UserProfile;
			var claim = profile != null ? Enumerable.From(profile.Claims).Where("$.Type == 'http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider'").FirstOrDefault() : null;
			var result = claim != null ? claim.Value : null;
			return result;
		});
	};

	return ctor;
});