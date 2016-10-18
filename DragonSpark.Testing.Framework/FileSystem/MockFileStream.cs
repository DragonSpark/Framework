using DragonSpark.Sources;
using System;
using System.IO;
using System.Linq;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	[Serializable]
	public class MockFileStream : MemoryStream
	{
		readonly private IFileSystemAccessor fileSystemAccessor;
		readonly private string path;

		public MockFileStream(IFileSystemAccessor fileSystemAccessor, string path, bool forAppend = false)
		{
			this.fileSystemAccessor = fileSystemAccessor;
			this.path = path;

			if (fileSystemAccessor.FileExists(path))
			{
				/* only way to make an expandable MemoryStream that starts with a particular content */
				var data = fileSystemAccessor.GetFile(path).Get();
				if (data != null && data.Length > 0)
				{
					Write(data.ToArray(), 0, data.Length);
					Seek(0, forAppend
						? SeekOrigin.End
						: SeekOrigin.Begin);
				}
			}
			else
			{
				fileSystemAccessor.AddFile(path, new FileElement(new byte[] { }));
			}
		}

		public sealed override long Seek( long offset, SeekOrigin loc ) => base.Seek( offset, loc );

		public sealed override void Write( byte[] buffer, int offset, int count ) => base.Write( buffer, offset, count );

		public override void Close()
		{
			InternalFlush();
		}

		public override void Flush()
		{
			InternalFlush();
		}

		private void InternalFlush()
		{
			if (fileSystemAccessor.FileExists(path))
			{
				var mockFileData = fileSystemAccessor.GetFile(path);
				/* reset back to the beginning .. */
				Seek(0, SeekOrigin.Begin);
				/* .. read everything out */
				var data = new byte[Length];
				Read(data, 0, (int)Length);
				/* .. put it in the mock system */
				mockFileData.Assign( data );
			}
		}
	}
}