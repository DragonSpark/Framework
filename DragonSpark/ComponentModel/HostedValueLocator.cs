using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.ComponentModel
{
	public sealed class HostedValueLocator<T> : ParameterizedSourceBase<ImmutableArray<T>> where T : class
	{
		public static HostedValueLocator<T> Default { get; } = new HostedValueLocator<T>();
		HostedValueLocator() {}

		public override ImmutableArray<T> Get( object parameter ) => parameter.GetAttributes<HostingAttributeBase>().Introduce( parameter, tuple => tuple.Item1.Get( tuple.Item2 ) ).OfType<T>().ToImmutableArray();
	}
}