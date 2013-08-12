using System.Collections.Generic;
using System.IO;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Io;

namespace DragonSpark.Server.Client
{
	public class InitializersBuilder : ClientModuleBuilder
	{
		public InitializersBuilder( IPathResolver pathResolver, string initialPath = "viewmodels" ) : base( pathResolver, initialPath )
		{}

		protected override IEnumerable<ClientModule> Create( FileInfo entryPoint, DirectoryInfo root )
		{
			var result = new DirectoryInfo( Path.Combine( entryPoint.DirectoryName, InitialPath ) ).EnumerateFiles( "*.initialize.js", SearchOption.AllDirectories ).Select( x => new ClientModule
			{
				Path = root.DetermineRelative( x, false ).Transform( z => Path.Combine( Path.GetDirectoryName( z ), Path.GetFileNameWithoutExtension(z) ).ToUri() )
			} ).ToArray();
			return result;
		}
	}
}