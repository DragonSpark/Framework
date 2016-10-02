using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public sealed class DefaultPropertyValueFactory : ParameterizedSourceBase<DefaultValueParameter, object>
	{
		public static DefaultPropertyValueFactory Default { get; } = new DefaultPropertyValueFactory();
		DefaultPropertyValueFactory() : this( HostedValueLocator<IDefaultValueProvider>.Default.ToSourceDelegate() ) {}

		readonly Func<MemberInfo, ImmutableArray<IDefaultValueProvider>> factory;

		public DefaultPropertyValueFactory( Func<MemberInfo, ImmutableArray<IDefaultValueProvider>> factory )
		{
			this.factory = factory;
		}

		public override object Get( DefaultValueParameter parameter ) => factory( parameter.Metadata ).Introduce( parameter, tuple => tuple.Item1.Get( tuple.Item2 ) ).FirstAssigned() ?? parameter.Metadata.From<DefaultValueAttribute, object>( attribute => attribute.Value );
	}
}