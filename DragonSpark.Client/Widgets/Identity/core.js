define( [ "dragonspark/configuration" ], function ( configuration ) {
	var instance = {
		claim : function (user, name) {
			var profile = user || configuration().UserProfile;
			var claim = profile != null ? Enumerable.From(profile.Claims).Where("$.Type == '{0}'".format(name) ).FirstOrDefault() : null;
			var result = claim != null ? claim.Value : null;
			return result;
		}
	}; 

	return instance;
});