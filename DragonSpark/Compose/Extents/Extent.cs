using DragonSpark.Model;

namespace DragonSpark.Compose.Extents;

public sealed class Extent
{
	public static Extent Default { get; } = new Extent();

	Extent() {}

	public SystemExtents System => SystemExtents.Default;

	public Extent<object> Any => Extent<object>.Default;

	public Extent<None> None => Extent<None>.Default;

	public Extent<T> Type<T>() => Extent<T>.Default;
}

public class Extent<T>
{
	public static Extent<T> Default { get; } = new Extent<T>();

	Extent() {}

	public ExtentSelection<T> Into => ExtentSelection<T>.Default;

	public Instance<T> Instance => Instance<T>.Implementation;
}