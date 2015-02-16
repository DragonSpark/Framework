using System.Collections.Generic;
using System.IO;

namespace DragonSpark.Server.Legacy.Io
{
	public interface ITemporaryFileRegistry
	{
		void Register( FileInfo fileInfo );

		IEnumerable<FileInfo> RegisteredFiles { get; }
	}
}