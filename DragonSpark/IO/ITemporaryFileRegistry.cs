using System.Collections.Generic;
using System.IO;

namespace DragonSpark.Io
{
	public interface ITemporaryFileRegistry
	{
		void Register( FileInfo fileInfo );

		IEnumerable<FileInfo> RegisteredFiles { get; }
	}
}