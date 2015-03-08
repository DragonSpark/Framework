using System;
using System.IO;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Properties;

namespace DragonSpark.Application.Presentation
{
	[Singleton( typeof(IResourceManager), Priority = Priority.Lowest )]
	public class DefaultResourceManager : IResourceManager
	{
		public Stream GetStream(Uri relativeUri, string assemblyName)
		{
			var uris = new[] { new Uri( string.Format( "{0};component{1}", assemblyName, relativeUri ), UriKind.Relative ), relativeUri };

			var resource = uris.Select( System.Windows.Application.GetResourceStream ).NotNull().FirstOrDefault();
		 	
			if (resource != null)
				return resource.Stream;

			DragonSpark.Runtime.Logging.Warning( string.Format( Resources.Message_DefaultResourceManager, relativeUri, assemblyName ) );
			return null;
		}
	}
}