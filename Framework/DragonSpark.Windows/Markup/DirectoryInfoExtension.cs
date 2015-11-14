using DragonSpark.Extensions;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	public class ReferenceExtension : Reference
	{
		public ReferenceExtension()
		{}

		public ReferenceExtension( string name ) : base( name )
		{}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var item = base.ProvideValue( serviceProvider );
			var result = item.AsTo<MarkupExtension, object>( extension => extension.ProvideValue( serviceProvider ) ) ?? item;
			return result;
		}
	}

	/*public class DirectoryInfoFactory : FactoryBase<string, DirectoryInfo>
	{
		readonly string baseDirectory;

		public DirectoryInfoFactory( string baseDirectory )
		{
			this.baseDirectory = baseDirectory ?? ;
		}

		protected override DirectoryInfo CreateFrom( Type resultType, string parameter )
		{
			// var directory = Directory.CreateDirectory( Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Reports" ) );
			
		}
	}*/

	[MarkupExtensionReturnType( typeof(string) )]
	public class NameExtension : MarkupExtension
	{
		public NameExtension() : this( null )
		{}

		public NameExtension( Type type )
		{
			Type = type;
		}

		public Type Type { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = Type.Transform( x => x.Name );
			return result;
		}
	}

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