using DragonSpark.Runtime.Environment;

namespace DragonSpark.Composition.Compose;

public class Locate<T> : LocateComponent<IComponentType, T>
{
	public static Locate<T> Default { get; } = new();

	Locate() : base(x => x) {}
}