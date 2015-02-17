using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Io
{
	public sealed class TemporaryFileRegistry : ITemporaryFileRegistry, IDisposable
	{
		readonly IList<FileInfo> files = new List<FileInfo>();


		void ITemporaryFileRegistry.Register( FileInfo fileInfo )
		{
			files.Add( fileInfo );
		}

		public IEnumerable<FileInfo> RegisteredFiles
		{
			get { return files.ToList().AsReadOnly(); }
		}
	
		public void Dispose()
		{
			files.Apply( x => x.Delete() );
		}

	}
}