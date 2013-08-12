using DragonSpark.Extensions;
using DragonSpark.Io;
using DragonSpark.Objects;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DragonSpark.Server.Client
{
	public abstract class ClientModuleBuilder : Factory<string, IEnumerable<ClientModule>>
	{
		readonly IPathResolver pathResolver;
		readonly string initialPath;

		protected ClientModuleBuilder( IPathResolver pathResolver, string initialPath )
		{
			this.pathResolver = pathResolver;
			this.initialPath = initialPath;
		}

		public string InitialPath
		{
			get { return initialPath; }
		}

		protected override IEnumerable<ClientModule> CreateItem( string parameter )
		{
			var entry = new FileInfo( pathResolver.Resolve( parameter ?? "~/Client/application.js"  ) );
			var root = new DirectoryInfo( Path.Combine( entry.DirectoryName, initialPath.ToStringArray( Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar ).First() ) );
			var result = Create( entry, root );
			return result;
		}

		protected abstract IEnumerable<ClientModule> Create( FileInfo entryPoint, DirectoryInfo root );
	}
}