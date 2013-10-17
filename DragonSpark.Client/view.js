define([ "require", "durandal/binder", "./Extensions/jquery", "./Extensions/knockout" ], function (require, binder) {
	var monitors = {};

	binder.throwOnErrors = true;

	var instance = window.$dsView = {
		overrides: [],
		ensureAttached: function (element, delegate) {
			var check = function () {
				if ($(element).parents(':last').is('html')) {
					setTimeout(delegate, 100);
					return true;
				}
				return false;
			};

			if (!check()) {
				var id = setInterval(function () {
					if (check()) {
						clearInterval(id);
					}
				}, 10);
			}
		},

		toHTML: function (text) {
			var charEncodings = {
				"\t": "&nbsp;&nbsp;&nbsp;&nbsp;",
				" ": "&nbsp;",
				"&": "&amp;",
				"<": "&lt;",
				">": "&gt;",
				"\n": "<br />",
				"\r": "<br />"
			};
			var space = /[\t ]/;
			var noWidthSpace = "&#8203;";
			text = (text || "") + "";  // make sure it's a string;
			text = text.replace(/\r\n/g, "\n");  // avoid adding two <br /> tags
			var html = "";
			var lastChar = "";
			for (var i in text) {
				if (!$.isFunction(text[i])) {
					var c = text[i];
					var charCode = text.charCodeAt(i);
					if (space.test(c) && !space.test(lastChar) && space.test(text[i + 1] || "")) {
						html += noWidthSpace;
					}
					html += c in charEncodings ? charEncodings[c] :
					charCode > 127 ? "&#" + charCode + ";" : c;
					lastChar = c;
				}
			}
			return html;
		},

		sizeMonitor: function (item) 
		{
			function create() 
			{
				var created = {
					width: ko.observable( "{0}px".format( item.width() ) ),
					height: ko.observable( "{0}px".format( item.height() ) )
				};
				item.bind( "resize", function (e) {
					var width = item.width(),
						height = item.height();
					created.width( "{0}px".format( width ) );
					created.height( "{0}px".format( height ) );
				} );
				return created;
			}

			var id = item.getUID();
			var result = monitors[id] || ( monitors[id] = create() );
			return result;
		}
	};
	return instance;
});