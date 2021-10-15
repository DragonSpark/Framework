using System;
using System.Reflection;

namespace DragonSpark.Compose.Extents;

public interface ISystemExtents
{
	Extent<Type> Type { get; }

	Extent<TypeInfo> Metadata { get; }
}