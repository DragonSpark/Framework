using System;
using System.Reflection;

namespace DragonSpark.Compose.Extents;

public sealed class SystemExtents : ISystemExtents
{
	public static SystemExtents Default { get; } = new SystemExtents();

	SystemExtents() {}

	public Extent<Type> Type => Extent<Type>.Default;

	public Extent<TypeInfo> Metadata => Extent<TypeInfo>.Default;
}