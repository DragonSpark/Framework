using System;
using System.Linq;
using System.Text;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public static class Defaults
	{
		public const string
			AllPattern = "*",
			PathRoot = @"Test:\";

		public static bool IsUnix { get; } = new[] { 4, 5, 128 }.Contains( (int)Environment.OSVersion.Platform );

		/// <summary>
		/// The default encoding.
		/// </summary>
		public static Encoding DefaultEncoding { get; } = new UTF8Encoding(false, true);

		/// <summary>
		/// Gets the default date time offset.
		/// E.g. for not existing files.
		/// </summary>
		public static DateTimeOffset DefaultDateTimeOffset { get; } = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc);

		public static Action<FileSystemRegistration> Register { get; } = RegisterFileSystemEntryCommand.Default.Execute;
	}
}