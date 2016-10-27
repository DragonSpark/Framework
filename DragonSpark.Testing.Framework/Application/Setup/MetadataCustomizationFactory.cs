using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class MetadataCustomizationFactory<T> : ParameterizedSourceBase<MethodBase, ImmutableArray<T>> where T : class
	{
		readonly static Func<object, IEnumerable<T>> CollectionSelector = HostedValueLocator<T>.Default.GetEnumerable;
		public static MetadataCustomizationFactory<T> Default { get; } = new MetadataCustomizationFactory<T>();
		MetadataCustomizationFactory() {}

		public override ImmutableArray<T> Get( MethodBase parameter )
		{
			var result = new object[] { parameter.DeclaringType.Assembly, parameter.DeclaringType, parameter }
				.SelectMany( CollectionSelector )
				.Prioritize()
				.ToImmutableArray();
			return result;
		}
	}
}