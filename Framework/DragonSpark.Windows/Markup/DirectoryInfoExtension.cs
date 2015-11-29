using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	/*public class DirectoryInfoFactory : FactoryBase<string, DirectoryInfo>
	{
		readonly string baseDirectory;

		public DirectoryInfoFactory( string baseDirectory )
		{
			this.baseDirectory = baseDirectory ?? ;
		}

		protected override DirectoryInfo CreateItem( Type resultType, string parameter )
		{
			// var directory = Directory.CreateDirectory( Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Reports" ) );
			
		}
	}*/

	[MarkupExtensionReturnType( typeof(DirectoryInfo) )]
	public class DirectoryInfoExtension : MarkupExtension
	{
		public DirectoryInfoExtension()
		{}

		public DirectoryInfoExtension( string path )
	    {
	        Path = path;
	    }

	    public string Path { get; set; }

	    public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var item = System.IO.Path.IsPathRooted( Path ) ? Path : System.IO.Path.GetFullPath( Path );
			var result = !DesignerProperties.GetIsInDesignMode( new DependencyObject() ) ? Directory.CreateDirectory( item ) : null;
			return result;
		}
	}
}