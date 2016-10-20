using System;
using System.IO;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public abstract class FileSystemElementBase : IFileSystemElement
	{
		/// <summary>
		/// Gets or sets the date and time the <see cref="FileElement"/> was created.
		/// </summary>
		public DateTimeOffset CreationTime { get; set; } = new DateTimeOffset( 2010, 01, 02, 00, 00, 00, TimeSpan.FromHours( 4 ) );

		/// <summary>
		/// Gets or sets the date and time of the <see cref="FileElement"/> was last accessed to.
		/// </summary>
		public DateTimeOffset LastAccessTime { get; set; } = new DateTimeOffset( 2010, 02, 04, 00, 00, 00, TimeSpan.FromHours( 4 ) );

		/// <summary>
		/// Gets or sets the date and time of the <see cref="FileElement"/> was last written to.
		/// </summary>
		public DateTimeOffset LastWriteTime { get; set; } = new DateTimeOffset( 2010, 01, 04, 00, 00, 00, TimeSpan.FromHours( 4 ) );

		/// <summary>
		/// Gets or sets the specified <see cref="FileAttributes"/> of the <see cref="FileElement"/>.
		/// </summary>
		public FileAttributes Attributes { get; set; } = FileAttributes.Normal;
	}
}