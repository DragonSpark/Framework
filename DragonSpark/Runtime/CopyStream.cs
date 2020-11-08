using DragonSpark.Model.Selection;
using System.IO;

namespace DragonSpark.Runtime
{
	public sealed class CopyStream : ISelect<Stream, MemoryStream>
	{
		public static CopyStream Default { get; } = new CopyStream();

		CopyStream() {}

		public MemoryStream Get(Stream parameter)
		{
			var result = new MemoryStream();
			parameter.Seek(0, SeekOrigin.Begin);
			parameter.CopyTo(result);
			return result;
		}
	}
}