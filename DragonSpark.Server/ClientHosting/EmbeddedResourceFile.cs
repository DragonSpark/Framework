using System.IO;
using System.Web.Hosting;

namespace DragonSpark.Server.ClientHosting
{
	public class EmbeddedResourceFile : VirtualFile
	{
		readonly Stream stream;

		public EmbeddedResourceFile( Stream stream, string virtualPath ) : base( virtualPath )
		{
			this.stream = stream;
		}

		public override Stream Open()
		{
			return stream;
		}
	}
}