using System;
using System.Net;
using DragonSpark.Setup;
using HtmlAgilityPack;
using Microsoft.Practices.Unity;

namespace DevelopersWin.VoteReporter
{
	[LifetimeManager( typeof(ContainerControlledLifetimeManager) )]
	public class DocumentProvider
	{
		public HtmlDocument Load( string location )
		{
			var result = Get( client => client.DownloadString( location ) );
			return result;
		}

		public HtmlDocument Load( Uri location )
		{
			var result = Get( client => client.DownloadString( location ) );
			return result;
		}

		static HtmlDocument Get( Func<WebClient, string> getContent )
		{
			using ( var client = new WebClient() )
			{
				var content = getContent( client );
				var result = new HtmlDocument();
				result.LoadHtml( content );
				return result;
			}
		}
	}
}