using System.IO;
using System.Windows.Markup;

namespace DragonSpark.Serialization
{
	public partial class XamlSerializationHelper
	{
		static TResult ResolveLoad<TResult>( Stream stream )
		{
			var xaml = new StreamReader( stream ).ReadToEnd();
			var result = Parse<TResult>( xaml );
			return result;
		}

		public static TResult Parse<TResult>( string xaml )
		{
			var result = (TResult)XamlReader.Load( xaml );
			return result;
		}
	}
}
