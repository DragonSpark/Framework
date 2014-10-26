using DragonSpark.Server.IO;
using System.IO;

namespace DragonSpark.Serialization
{
	public class FileStreamResolver : IStreamResolver
	{
		readonly string filePath;

		public FileStreamResolver( string filePath )
		{
			this.filePath = filePath;
		}

		public string FilePath
		{
			get { return filePath; }
		}

		public Stream ResolveStream()
		{
			var filepath = PathSupport.ResolvePath( FilePath );
			var result = File.OpenRead( filepath );
			return result;
		}
	}
}