using DragonSpark.Model.Selection.Stores;
using System.IO;

namespace DragonSpark.Runtime
{
	sealed class BytesToStreamSelector : ActivatedStore<byte[], MemoryStream>
	{
		public static BytesToStreamSelector Default { get; } = new BytesToStreamSelector();

		BytesToStreamSelector() {}
	}
}