using DragonSpark.Extensions;
using DragonSpark.Io;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DragonSpark.Server.Client
{
	public class CommandsBuilder : ClientModuleBuilder
	{
		public CommandsBuilder( IPathResolver pathResolver, string initialPath = "commands" ) : base( pathResolver, initialPath )
		{}

		protected override IEnumerable<ClientModule> Create( FileInfo entryPoint, DirectoryInfo root )
		{
			var result = new DirectoryInfo( Path.Combine( entryPoint.DirectoryName, InitialPath ) ).EnumerateFiles( "*.js", SearchOption.AllDirectories ).Select( x => new ClientModule
				{
					Path = root.DetermineRelative( x, false ).Transform( z => Path.Combine( Path.GetDirectoryName( z ), Path.GetFileNameWithoutExtension(z) ).ToUri() )
				} ).ToArray();
			return result;
		}
	}
}