using System;
using System.IO;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public abstract class FileSystemInfoBase<T> : IFileSystemInfo where T : FileSystemInfoBase
	{
		protected FileSystemInfoBase( T source )
		{
			Source = source;
		}

		protected T Source { get; }

		public void Delete() => Source.Delete();

		public void Refresh() => Source.Refresh();

		public FileAttributes Attributes
		{
			get { return Source.Attributes; }
			set { Source.Attributes = value; }
		}

		public DateTime CreationTime
		{
			get { return Source.CreationTime; }
			set { Source.CreationTime = value; }
		}

		public DateTime CreationTimeUtc
		{
			get { return Source.CreationTimeUtc; }
			set { Source.CreationTimeUtc = value; }
		}

		public bool Exists => Source.Exists;

		public string Extension => Source.Extension;

		public string FullName => Source.FullName;

		public DateTime LastAccessTime
		{
			get { return Source.LastAccessTime; }
			set { Source.LastAccessTime = value; }
		}

		public DateTime LastAccessTimeUtc
		{
			get { return Source.LastAccessTimeUtc; }
			set { Source.LastAccessTimeUtc = value; }
		}

		public DateTime LastWriteTime
		{
			get { return Source.LastWriteTime; }
			set { Source.LastWriteTime = value; }
		}

		public DateTime LastWriteTimeUtc
		{
			get { return Source.LastWriteTimeUtc; }
			set { Source.LastWriteTimeUtc = value; }
		}

		public string Name => Source.Name;
	}
}