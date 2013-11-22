using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;

namespace DragonSpark.Server.ClientHosting
{
	public class SignalRBundleFile : BundleFile
	{
		const string SignalRPath = "~/signalr/hubs";

		public SignalRBundleFile() : base( SignalRPath, new SignalRVirtualFile() )
		{}

		class SignalRVirtualFile : VirtualFile
		{
			public SignalRVirtualFile() : base( SignalRPath )
			{}

			public override Stream Open()
			{
				var url = new Uri( ServerContext.Current.Request.Url, VirtualPathUtility.ToAbsolute( SignalRPath ) );
				var result = new WebClient().OpenRead( url );
				return result;
			}
		}
	}
}