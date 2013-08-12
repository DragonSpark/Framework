using DragonSpark.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.Hosting;

namespace DragonSpark.Server.Client
{
	public class VirtualPathProvider : System.Web.Hosting.VirtualPathProvider
	{
		public const string Path = "~/Resources/Embedded";

		readonly System.Web.Hosting.VirtualPathProvider contained;

		public VirtualPathProvider( System.Web.Hosting.VirtualPathProvider contained )
		{
			this.contained = contained;
		}

		public override bool FileExists( string virtualPath )
		{
			var result = IsEmbeddedPath( virtualPath ) || contained.FileExists( virtualPath );
			return result;
		}

		public override CacheDependency GetCacheDependency( string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart )
		{
			var result = !IsEmbeddedPath( virtualPath ) ? contained.GetCacheDependency( virtualPath, virtualPathDependencies, utcStart ) : null;
			return result;
		}

		public override VirtualDirectory GetDirectory( string virtualDir )
		{
			return contained.GetDirectory( virtualDir );
		}

		public override bool DirectoryExists( string virtualDir )
		{
			return contained.DirectoryExists( virtualDir );
		}

		public override VirtualFile GetFile( string virtualPath )
		{
			var result = IsEmbeddedPath( virtualPath ) ? DetermineFile( virtualPath ) : contained.GetFile( virtualPath );
			return result;
		}

		static VirtualFile DetermineFile( string virtualPath )
		{
			var parts = new Queue<string>( virtualPath.Replace( Path, string.Empty ).ToStringArray( System.IO.Path.AltDirectorySeparatorChar ) );
			var assemblyName = parts.Dequeue();
			var path = string.Join( System.IO.Path.AltDirectorySeparatorChar.ToString(), parts.ToArray() );
			var stream = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault( x => x.GetName().Name == assemblyName ).Transform( x => x.GetManifestResourceStream( path ) );
			return new EmbeddedResourceFile( stream, virtualPath );
		}

		static bool IsEmbeddedPath( string path )
		{
			var result = path.StartsWith( Path );
			return result;
		}
	}
}