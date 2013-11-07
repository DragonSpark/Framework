using DragonSpark.Io;
using System;
using System.Windows.Markup;

namespace DragonSpark.Configuration
{
	[MarkupExtensionReturnType( typeof(string) )]
	public class PathSupportExtension : MarkupExtension
	{
		public PathSupportExtension( string path )
		{
			Path = path;
		}

		public string Path { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = PathSupport.ResolvePath( Path );
			return result;
		}
	}
}
