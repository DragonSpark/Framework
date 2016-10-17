using System.IO;
using System.Windows.Markup;

namespace DragonSpark.Windows.Legacy.Markup
{
	[MarkupExtensionReturnType( typeof(DirectoryInfo) )]
	public class DirectoryInfoExtension : MarkupExtensionBase
	{
		public DirectoryInfoExtension() {}

		public DirectoryInfoExtension( string path )
	    {
	        Path = path;
	    }

	    public string Path { get; set; }

		protected override object GetValue( MarkupServiceProvider serviceProvider )
		{
			var item = System.IO.Path.IsPathRooted( Path ) ? Path : System.IO.Path.GetFullPath( Path );
			var result = Directory.CreateDirectory( item );
			return result;
		}
	}
}