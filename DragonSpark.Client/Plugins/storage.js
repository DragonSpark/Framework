define( [ "./serializer" ], function( serializer ) {
	var instance = 
		{
			register : function( key, initial )
			{
				var result = ko.observable();
				var current = instance.get( key );
				var assign = current !== null ? current : initial;
				result( assign );

				result.subscribe( function( value ) {
					instance.set( key, value );
				} );

				return result;
			},
		
			get : function( key )
			{
				var saved = localStorage.getItem( key );
				var result = saved ? serializer.deserialize( saved ) : null;
				return result;
			},
			set : function( key, value )
			{
				var result = serializer.serialize( value );
				localStorage[ key ] = result;
				return result;
			}
		};
	return instance;
});