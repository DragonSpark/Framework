using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public sealed class DefaultPropertyValueFactory : ParameterizedSourceBase<PropertyInfo, object>
	{
		public static DefaultPropertyValueFactory Default { get; } = new DefaultPropertyValueFactory();
		DefaultPropertyValueFactory() : this( HostedValueLocator<IDefaultValueProvider>.Default.ToDelegate() ) {}

		readonly Func<MemberInfo, ImmutableArray<IDefaultValueProvider>> factory;

		public DefaultPropertyValueFactory( Func<MemberInfo, ImmutableArray<IDefaultValueProvider>> factory )
		{
			this.factory = factory;
		}

		public override object Get( PropertyInfo parameter ) => factory( parameter ).Introduce( parameter, tuple => tuple.Item1.Get( tuple.Item2 ) ).FirstAssigned() ?? parameter.From<DefaultValueAttribute, object>( attribute => attribute.Value );
	}
}