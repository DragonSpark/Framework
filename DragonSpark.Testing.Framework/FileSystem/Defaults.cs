using System;
using System.IO;
using System.Text;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public static class Defaults
	{
		/// <summary>
		/// The default encoding.
		/// </summary>
		public static Encoding DefaultEncoding { get; } = new UTF8Encoding(false, true);

		/// <summary>
		/// Gets the default date time offset.
		/// E.g. for not existing files.
		/// </summary>
		public static DateTimeOffset DefaultDateTimeOffset { get; } = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc);

		public static string DirectoryName { get; } = Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString() );
	}
}