using System.Windows.Markup;
using DragonSpark.Io;

namespace DragonSpark.Configuration
{
	[ContentProperty( "Path" )]
	public partial class PathHelperExtension : MarkupExtension
	{
		public PathHelperExtension( string path )
		{
			Path = path;
		}

		public string Path { get; set; }

		public override object ProvideValue( System.IServiceProvider serviceProvider )
		{
			var result = PathHelper.ResolvePath( Path );
			return result;
		}
	}
}