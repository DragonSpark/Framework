using System.IO;
using DragonSpark.Io;

namespace DragonSpark.Serialization
{
	public class FileStreamResolver : IStreamResolver
	{
		readonly string filePath;

		public FileStreamResolver( string filePath )
		{
			this.filePath = filePath;
		}

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( !string.IsNullOrEmpty( filePath ) );
		}*/

		public string FilePath
		{
			get { return filePath; }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Stream is disposed elsewhere." )]
		public Stream ResolveStream()
		{
			var filepath = PathSupport.ResolvePath( FilePath );
			var result = File.OpenRead( filepath );
			return result;
		}
	}
}