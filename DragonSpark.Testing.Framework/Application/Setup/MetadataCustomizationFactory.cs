using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class MetadataCustomizationFactory<T> : ParameterizedSourceBase<MethodBase, ImmutableArray<T>> where T : class
	{
		public static MetadataCustomizationFactory<T> Default { get; } = new MetadataCustomizationFactory<T>();
		MetadataCustomizationFactory() {}

		public override ImmutableArray<T> Get( MethodBase parameter )
		{
			var result = new object[] { parameter.DeclaringType.Assembly, parameter.DeclaringType, parameter }
				.SelectMany( o => HostedValueLocator<T>.Default.Get( o ).AsEnumerable() )
				.Prioritize()
				.ToImmutableArray();
			return result;
		}
	}
}