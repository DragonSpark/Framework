using System;
using System.Collections.Immutable;
using System.Reflection;
using DragonSpark.Composition;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public class ContainingTypeAndNestedAttribute : TypeProviderAttributeBase
	{
		readonly static Func<MethodBase, ImmutableArray<Type>> Delegate = Factory.Default.Get;
		public ContainingTypeAndNestedAttribute() : base( Delegate ) {}

		new sealed class Factory : ParameterizedSourceBase<MethodBase, ImmutableArray<Type>>
		{
			public static Factory Default { get; } = new Factory();
			Factory() {}

			public override ImmutableArray<Type> Get( MethodBase parameter ) => SelfAndNestedTypes.Default.Get( parameter.DeclaringType ).ToImmutableArray();
		}
	}
}