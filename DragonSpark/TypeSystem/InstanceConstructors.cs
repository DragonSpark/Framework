using DragonSpark.Sources.Parameterized.Caching;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	sealed class InstanceConstructors : ArgumentCache<TypeInfo, ImmutableArray<ConstructorInfo>>
	{
		public static InstanceConstructors Default { get; } = new InstanceConstructors();
		InstanceConstructors() : base( info => info.DeclaredConstructors.Where( constructorInfo => constructorInfo.IsPublic && !constructorInfo.IsStatic ).ToImmutableArray() ) {}
	}
}