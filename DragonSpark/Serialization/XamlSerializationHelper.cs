using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace DragonSpark.Serialization
{
	static partial class XamlSerializationHelper
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
	}
}
