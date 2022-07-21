using DragonSpark.Compose;

namespace DragonSpark.Model.Sequences;

public class Append<T> : Instances<T>
{
	protected Append(IArray<T> previous, T next) : base(previous.Get().Open().Append(next)) {}
}