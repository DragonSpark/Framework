using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Windows;
using System.Windows.Resources;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Logging;

namespace DragonSpark.Server.ClientHosting
{
	public interface IClientResourceStreamProvider
	{
		Stream Retrieve( ClientResourceInformation information );
	}

	public class ClientResourceInformation
	{
		readonly string virtualPath;
		readonly Assembly assembly;
		readonly string fileName;

		public ClientResourceInformation( string virtualPath, Assembly assembly, string fileName )
		{
			this.virtualPath = virtualPath;
			this.assembly = assembly;
			this.fileName = fileName;
		}

		public string VirtualPath
		{
			get { return virtualPath; }
		}

		public Assembly Assembly
		{
			get { return assembly; }
		}

		public string FileName
		{
			get { return fileName; }
		}
	}

	public interface IClientResourceInformationProvider
	{
		ClientResourceInformation Retrieve( string path );

		IEnumerable<ClientResourceInformation> RetrieveAll( string path );
	}

	[Singleton( typeof(IClientResourceInformationProvider) )]
	class ClientResourceInformationProvider : IClientResourceInformationProvider
	{
		readonly IEnumerable<Assembly> assemblies;
		readonly IDictionary<string, ClientResourceInformation> cache = new Dictionary<string, ClientResourceInformation>();

		public ClientResourceInformationProvider( IEnumerable<ClientResourceManifest> manifests )
		{
			assemblies = manifests.Select( x => x.GetType().Assembly ).ToArray();
		}

		public ClientResourceInformation Retrieve( string path )
		{
			var result = cache.Ensure( path, Parse );
			return result;
		}

		static bool InDirectory( Assembly assembly, string directory )
		{
			var result = assembly.GetResourceNames().Select( x => Path.GetDirectoryName( x ).Replace( Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar ) ).Any( x => x.Equals( directory, StringComparison.InvariantCultureIgnoreCase ) );
			return result;
		}

		static bool Exists( Assembly assembly, string file )
		{
			var items = assembly.GetResourceNames();
			var result = items.Concat( items.GroupBy( Path.GetDirectoryName ).Select( x => x.Key ) ).Distinct().Select( x => x.Replace( Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar ) ).Any( x => x.Equals( file, StringComparison.InvariantCultureIgnoreCase ) );
			return result;
		}

		public IEnumerable<ClientResourceInformation> RetrieveAll( string path )
		{
			var information = Retrieve( path );
			var result = information.Transform( x => x.Assembly.GetResourceNames().Where( y => InDirectory( x.Assembly, x.FileName ) ).Select( y => Retrieve( string.Format( "~/{0};component/{1}", x.Assembly.GetName().Name, y ) ) ) ).ToArray();
			return result;
		}

		ClientResourceInformation Parse( string path )
		{
			var input = VirtualPathUtility.ToAppRelative( path );
			var parts = VirtualPathUtility.ToAbsolute( input ).Replace( VirtualPathProvider.Qualifier, string.Empty ).Trim( Path.AltDirectorySeparatorChar ).Split( new[] { ";component/" }, StringSplitOptions.RemoveEmptyEntries );
			switch ( parts.Length )
			{
				case 2:
					var result = assemblies.FirstOrDefault( x => x.GetName().Name == parts.First() ).Transform( x =>
					{
						var fileName = parts.Last();
						var appRelative = VirtualPathUtility.ToAppRelative( input );
						var item = Exists( x, fileName ) ? new ClientResourceInformation( appRelative, x, fileName ) : null;
						return item;
					} );
					return result;
			}
			return null;
		}
	}

	[Singleton( typeof(IClientResourceStreamProvider), Priority = Priority.Lowest )]
	public class ClientResourceStreamProvider : IClientResourceStreamProvider
	{
		protected virtual Stream DetermineStream( ClientResourceInformation information )
		{
			var uriResource = new Uri( string.Format( "pack://application:,,,/{0};component/{1}", information.Assembly.GetName().Name, information.FileName ) );
			var result = Application.GetResourceStream( uriResource ).Stream;
			return result;
		}

		public Stream Retrieve( ClientResourceInformation information )
		{
			try
			{
				var result = DetermineStream( information );
				return result;
			}
			catch ( Exception e )
			{
				Log.Warning( string.Format( "Stream not found for assembly '{0}', file: {1}.  Reason: '{2}'.", information.Assembly.GetName(), information.FileName, e.Message ) );
				return null;
			}
		}
	}
}