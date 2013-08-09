using DragonSpark.Extensions;
using DragonSpark.Properties;
using System;
using System.IO;

namespace DragonSpark.Io
{
	public partial class Workspace : IWorkspace
	{
		readonly DirectoryInfo directory;

		public static Workspace Create( string directoryPath = null )
		{
			var result = directoryPath.Transform( x => new Workspace( x ) ) ?? Create( Guid.NewGuid() );
			return result;
		}

        public static Workspace Create( Guid guid )
		{
			var path = Path.Combine( Path.GetTempPath(), guid.ToString() );
			var result = new Workspace( path );
			return result;
		}

		public Workspace( string directory )
		{
			this.directory = System.IO.Directory.CreateDirectory( directory );
			Logging.Log.Information( string.Format( Resources.Message_Workspace_Created, this.directory.Name ) );
		}

		public string Write( string fileName, string contents )
		{
			var result = Path.Combine( Directory.FullName, fileName );
			File.WriteAllText( result, contents );
			return result;
		}

	    public string Save( string fileName, Stream stream )
	    {
			var result = Path.IsPathRooted( fileName ) ? fileName : Path.Combine( Directory.FullName, fileName );
            System.IO.Directory.CreateDirectory( Path.GetDirectoryName( result ) );
	        using ( var file = File.Create( result ) )
	        {
	            CopyStream( stream, file ); 
            }
	        return result;
	    }

	    static void CopyStream( Stream input, Stream output )
	    {
	        var buffer = new byte[8 * 1024];
	        int len;
	        while ( ( len = input.Read( buffer, 0, buffer.Length ) ) > 0 )
	        {
	            output.Write( buffer, 0, len );
	        }
	    } 

		public void Copy( string sourceFilePath )
		{
			var destination = Path.Combine( Directory.FullName, Path.GetFileName( sourceFilePath ) );
			File.Copy( sourceFilePath, destination, true );
		}

		public DirectoryInfo Directory
		{
			get { return directory; }
		}

		public void Dispose() 
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			Logging.Log.Try( () =>
			{
				Directory.Delete( true );
				Logging.Log.Information( string.Format( Resources.Message_Workspace_Dispose, directory ) );
			} //, error => string.Format( "Could not delete directory '{0}' due to the following reason: '{1}'.", directory, error.Message ) 
			);
		}
	}
}
