using System;
using Xamarin.Forms.Xaml;

namespace DragonSpark.Client
{
	public class HelloWorldExtension : IMarkupExtension
	{
		/*readonly string message;

		public HelloWorldExtension( string message )
		{
			this.message = message;
		}*/

		public string Message { get; set; }

		public object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = string.Format( "Hello World! Message: {0}", Message );
			return result;
		}
	}
}