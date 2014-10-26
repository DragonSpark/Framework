using System.IO;
using System.Windows.Markup;

namespace DragonSpark.Serialization
{
	public static class XamlSerializationSupport
	{
		public static string ToXaml( object target )
		{
			using ( var writer = new StringWriter() )
			{
				XamlWriter.Save( target, writer );
				var result = writer.GetStringBuilder().ToString();
				return result;
			}
		}

		public static void Save( object target, string filePath )
		{
			using ( var file = File.Create( filePath ) )
			{
				XamlWriter.Save( target, file );
			}
		}

		public static TResult Parse<TResult>( string xaml )
		{
			var result = (TResult)XamlReader.Parse( xaml );
			return result;
		}

		static TResult ResolveLoad<TResult>( Stream stream )
		{
			var result = (TResult)XamlReader.Load( stream );
			return result;
		}

		public static TResult Load<TResult>( Stream stream )
		{
			if ( stream != null )
			{
				using ( stream )
				{
					var result = ResolveLoad<TResult>( stream );
					return result;
				}
			}
			return default(TResult);
		}

		public static TResult Load<TResult>( IStreamResolver resolver )
		{
			var stream = resolver.ResolveStream();
			var result = Load<TResult>( stream );
			return result;
		}

		public static object Load( Stream stream )
		{
			var result = Load<object>( stream );
			return result;
		}

		public static object Load( IStreamResolver resolver )
		{
			var result = Load<object>( resolver );
			return result;
		}
	}
}
