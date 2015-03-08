using System;
using System.Windows.Browser;

namespace DragonSpark.Application.Presentation
{
	public static class Cookie
	{
		/// 
		/// sets a persistent cookie with huge expiration date
		/// 
		/// the cookie key
		/// the cookie value
		public static void SetCookie( string key, string value )
		{
			var expiration = DateTime.UtcNow + TimeSpan.FromDays( 2000 );
			var cookie = String.Format( "{0}={1};expires={2}", key, value, expiration.ToString( "R" ) );
			HtmlPage.Document.SetProperty( "cookie", cookie );
		}

		/// 
		/// Retrieves an existing cookie
		/// 
		/// cookie key
		/// null if the cookie does not exist, otherwise the cookie value
		public static string GetCookie( string key )
		{
			var cookies = HtmlPage.Document.Cookies.Split( ';' );
			key += '=';
			foreach ( var cookie in cookies )
			{
				var cookieStr = cookie.Trim();
				if ( cookieStr.StartsWith( key, StringComparison.OrdinalIgnoreCase ) )
				{
					var vals = cookieStr.Split( '=' );

					if ( vals.Length >= 2 )
					{
						return vals[ 1 ];
					}

					return string.Empty;
				}
			}

			return null;
		}

		/// 
		/// Deletes a specified cookie by setting its value to empty and expiration to -1 days
		/// 
		/// the cookie key to delete
		public static void DeleteCookie( string key )
		{
			// HtmlPage.Document.GetProperty( "cookie" ) as String;
			var expiration = DateTime.UtcNow - TimeSpan.FromDays( 1 );
			var cookie = String.Format( "{0}=;expires={1}", key, expiration.ToString( "R" ) );
			HtmlPage.Document.SetProperty( "cookie", cookie );
		}
	}
}