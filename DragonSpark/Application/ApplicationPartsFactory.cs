using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Application
{
	public class ApplicationPartsFactory : ConfiguringFactory<ImmutableArray<Type>, SystemParts>
	{
		public static ApplicationPartsFactory Default { get; } = new ApplicationPartsFactory();
		ApplicationPartsFactory() : this( SystemPartsFactory.Default.Get, ApplicationParts.Default.Assign ) {}

		[UsedImplicitly]
		public ApplicationPartsFactory( Func<ImmutableArray<Type>, SystemParts> factory, Action<SystemParts> configure ) : base( factory, configure ) {}
	}
}