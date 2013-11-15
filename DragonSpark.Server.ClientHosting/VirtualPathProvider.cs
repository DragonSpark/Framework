using System.Reflection;
using DragonSpark.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.Hosting;

namespace DragonSpark.Server.ClientHosting
{
	public class VirtualPathProvider : System.Web.Hosting.VirtualPathProvider
	{
		public const string Qualifier = "clientResource~/", Application = "~/DragonSpark/Application.js";

		readonly IClientResourceStreamProvider provider;
		readonly IClientResourceInformationProvider informationProvider;
		readonly System.Web.Hosting.VirtualPathProvider inner;

		public VirtualPathProvider( IClientResourceStreamProvider provider, IClientResourceInformationProvider informationProvider, System.Web.Hosting.VirtualPathProvider inner )
		{
			this.provider = provider;
			this.informationProvider = informationProvider;
			this.inner = inner;
		}

		public override bool FileExists( string virtualPath )
		{
			var information = informationProvider.Retrieve( virtualPath );
			var result = information != null || inner.FileExists( virtualPath );
			return result;
		}

		public override CacheDependency GetCacheDependency( string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart )
		{
			var information = informationProvider.Retrieve( virtualPath );
			var result = information == null ? inner.GetCacheDependency( virtualPath, virtualPathDependencies, utcStart ) : null;
			return result;
		}

		public override VirtualDirectory GetDirectory( string virtualDir )
		{
			return inner.GetDirectory( virtualDir );
		}

		public override bool DirectoryExists( string virtualDir )
		{
			return inner.DirectoryExists( virtualDir );
		}

		public override VirtualFile GetFile( string virtualPath )
		{
			var result = informationProvider.Retrieve( virtualPath ).Transform( x => new ClientResourceFile( provider, virtualPath ) ) ?? inner.GetFile( virtualPath );
			return result;
		}
	}
}