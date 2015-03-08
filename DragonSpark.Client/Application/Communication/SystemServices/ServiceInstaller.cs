using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.Automation;
using System.ServiceModel;
using DragonSpark.Extensions;
using DragonSpark.Io;

namespace DragonSpark.Application.Communication.SystemServices
{
    public class ServiceInstaller<TService> : IServiceInstaller
    {
        readonly IDownloader _downloader;
        readonly IWorkspace _workspace;
        readonly Uri _uri = new ChannelFactory<TService>( "*" ).Endpoint.Address.Uri;

        public ServiceInstaller( IDownloader downloader, IWorkspace workspace )
        {
            _downloader = downloader;
            _workspace = workspace;
        }

        Version ResolveVersion()
        {
            var result = Retrieve( "Version", "TXT" ).Transform( x => new StreamReader( x ).ReadToEnd().Transform( y => new Version( y ) ) );
            return result;
        }

        Stream Retrieve( string type, string extension )
        {
            var file = new Uri( string.Format( ".{0}.{1}.{2}", _uri.AbsolutePath, type, extension ), UriKind.Relative );
            try
            {
                var result = _downloader.Retrieve( file );
                return result;
            }
            catch ( WebException )
            {
                return null;
            }
        }

        public ServiceInstallation Install()
        {
            var filePath = ResolveVersion().Transform( x => Path.Combine( _workspace.Directory.FullName, x.ToString(), Path.ChangeExtension( Path.GetFileName( _uri.AbsolutePath ), ".EXE" ) ) );

            if ( !File.Exists( filePath ) )
            {
                var stream = Retrieve( "Installer", "EXE" );
                var installPath = Path.ChangeExtension( filePath, "Installer.EXE" );
                var save = _workspace.Save( installPath, stream );
                    
                using ( var shell = AutomationFactory.CreateObject( "WScript.Shell" ) )
                {
                    var format = string.Format( @"{0} /T:""{1}"" /Q", save, Path.GetDirectoryName( filePath ) );
                    shell.Run( format, 0, true );
                }
            }

            var result = new ServiceInstallation( filePath );
            result.Start();
            return result;
        }
    }
}