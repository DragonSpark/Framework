define( [ "require" ], function( r ) {
	var ctor = function() {};
	
	ctor.prototype.path = function( path )
	{
		var result = r.toUrl( path );
		return result;
	};
	
	return ctor;
} );