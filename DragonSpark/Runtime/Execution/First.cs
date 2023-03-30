using DragonSpark.Model;
using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Execution;

public sealed class First : FirstBase
{
	public static implicit operator bool(First instance) => instance.Get(None.Default);

	public First() : this(new Variable<int>()) {}

	public First(IMutable<int> store) : base(new Counter(store)) {}
}