using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using Antlr.Runtime.Misc;

namespace DragonSpark.Server.ClientHosting
{
	class ClientResourceVirtualDirectory : VirtualDirectory
	{
		readonly IEnumerable<ClientResourceFile> files;

		public ClientResourceVirtualDirectory( string virtualPath, IEnumerable<ClientResourceFile> files ) : base( virtualPath )
		{
			this.files = files;
		}

		public override IEnumerable Directories
		{
			get { throw new System.NotImplementedException(); }
		}

		public override IEnumerable Files
		{
			get { return files; }
		}

		public override IEnumerable Children
		{
			get { throw new System.NotImplementedException(); }
		}
	}

	public class ClientResourceFile : VirtualFile
	{
		readonly Func<Stream> resolver;

		public ClientResourceFile( string path, Func<Stream> resolver ) : base( path )
		{
			this.resolver = resolver;
		}

		public override Stream Open()
		{
			var result = resolver();
			return result;
		}
	}
}