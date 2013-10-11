define( [ "jquery" ], function( $ ) {
	$.fn.highlight = function()
	{
		this.focus();
		this.select();
	};

    var uid = 0;
    $.getUID = function() {
        uid++;
        return 'jQ-uid-'+uid;
    };
	$.fn.getUID = function()
	{
		if ( !this.length )
		{
			return 0;
		}
		var fst = this.first(), id = fst.attr( 'id' );
		if ( !id )
		{
			id = $.getUID();
			fst.attr( 'id', id );
		}
		return id;
	};
	return {};
} );