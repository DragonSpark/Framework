using AsyncUtilities;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Operations;

public sealed class Locks : ReferenceValueStore<object, AsyncLock>
{
	public static Locks Default { get; } = new();

	Locks() : base(_ => new ()) {}
}