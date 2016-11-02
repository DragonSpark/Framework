using DragonSpark.Sources.Scopes;
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
		readonly private IFileSystemRepository repository;
		readonly private string path;

		public MockFileStream(IFileSystemRepository repository, string path, bool forAppend = false)
		{
			this.repository = repository;
			this.path = path;

			if (repository.Contains(path))
			{
				/* only way to make an expandable MemoryStream that starts with a particular content */
				var data = repository.GetFile(path).Get();
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
				repository.Set( path, FileElement.Empty() );
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
			if (repository.Contains(path))
			{
				var mockFileData = repository.GetFile(path);
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