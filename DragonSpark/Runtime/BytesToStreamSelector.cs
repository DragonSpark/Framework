using System.IO;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Runtime
{
	sealed class BytesToStreamSelector : ActivatedStore<byte[], MemoryStream>
	{
		public static BytesToStreamSelector Default { get; } = new BytesToStreamSelector();

		BytesToStreamSelector() {}
	}
}