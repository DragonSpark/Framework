using System.Web.Hosting;
using System.Web.Optimization;
using DragonSpark.Extensions;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace DragonSpark.Server.ClientHosting
{
	public class ClientResourceHttpHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return true; }
		}

		void IHttpHandler.ProcessRequest( HttpContext context )
		{
			BundleTable.VirtualPathProvider.GetFile( context.Request.AppRelativeCurrentExecutionFilePath ).NotNull( x =>
			{
				context.Response.Clear();
				context.Response.ContentType = MimeMapping.GetMimeMapping( x.Name );
				x.Open().CopyTo( context.Response.OutputStream );
			} );
		}
	}
}