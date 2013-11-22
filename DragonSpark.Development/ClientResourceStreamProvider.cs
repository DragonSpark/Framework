using System.IO;
using System.Text;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Server.ClientHosting;
using Microsoft.Cci;

namespace DragonSpark.Development
{
	[Singleton( typeof(IClientResourceStreamProvider) )]
    public class ClientResourceStreamProvider : Server.ClientHosting.ClientResourceStreamProvider
    {
		protected override Stream DetermineStream( ClientResourceInformation information )
		{
			using ( var host = new PeReader.DefaultHost() )
			{
				return host.LoadUnitFrom( information.Assembly.Location ).AsTo<IAssembly, Stream>( y =>
				{
					var file = new FileInfo( Path.Combine( Path.GetDirectoryName( y.DebugInformationLocation ), @"..\..", information.FileName ) );

					var stream = file.Exists ? new MemoryStream( DetermineBytes( file ) ) : null;
					return stream;
				} );
			}
		}

		
		static byte[] DetermineBytes( FileInfo file )
		{
			var bytes = File.ReadAllBytes( file.FullName );
			switch ( Path.GetExtension( file.FullName ) )
			{
				case ".map":
					var data = Encoding.Default.GetString( bytes ).Replace( @"""sourceRoot"":""""", string.Format( @"""sourceRoot"":""{0}/""", file.DirectoryName.Replace( Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar ) ) );
					return Encoding.Default.GetBytes( data );
			}
			return bytes;
		}
    }
}
