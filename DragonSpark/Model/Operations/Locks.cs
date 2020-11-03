using AsyncUtilities;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Operations
{
	public sealed class Locks : ReferenceValueStore<object, AsyncLock>
	{
		public static Locks Default { get; } = new Locks();

		Locks() : base(_ => new AsyncLock()) {}
	}
}