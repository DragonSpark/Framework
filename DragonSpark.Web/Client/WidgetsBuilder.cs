using DragonSpark.Extensions;
using DragonSpark.Io;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DragonSpark.Web.Client
{
	public class WidgetsBuilder : ClientModuleBuilder
	{
		public WidgetsBuilder( IPathResolver pathResolver, string initialPath = "widgets" ) : base( pathResolver, initialPath )
		{}

		protected override IEnumerable<ClientModule> Create( FileInfo entryPoint, DirectoryInfo root )
		{
			var result = new DirectoryInfo( Path.Combine( entryPoint.DirectoryName, InitialPath ) ).EnumerateDirectories().Select( x => new WidgetModule
				{
					Name = x.Name,
					Path = x.GetFiles( "*.initialize.js" ).FirstOrDefault()
					       .Transform( y => root.DetermineRelative( y, false )
					                            .Transform( z => Path.Combine( Path.GetDirectoryName( z ), Path.GetFileNameWithoutExtension(z) ).ToUri() ) )
				} ).ToArray();
			return result;	
		}
	}
}