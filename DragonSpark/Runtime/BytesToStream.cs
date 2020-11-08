using DragonSpark.Model.Selection.Stores;
using System.IO;

namespace DragonSpark.Runtime
{
	sealed class BytesToStream : ActivatedStore<byte[], MemoryStream>
	{
		public static BytesToStream Default { get; } = new BytesToStream();

		BytesToStream() {}
	}
}