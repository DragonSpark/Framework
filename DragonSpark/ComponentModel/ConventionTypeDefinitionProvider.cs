using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DragonSpark.ComponentModel
{
	public sealed class ConventionTypeDefinitionProvider : AlterationBase<TypeInfo>, ITypeDefinitionProvider
	{
		public static ConventionTypeDefinitionProvider Default { get; } = new ConventionTypeDefinitionProvider();
		ConventionTypeDefinitionProvider() {}

		public override TypeInfo Get( TypeInfo parameter )
		{
			var context = new Context( parameter );
			var result = context.Loop( 
				item => item.CreateFromBaseType(), 
				item => item.Metadata != null,
				item => item.Metadata
				);
			return result;
		}

		struct Context
		{
			readonly static ICache<TypeInfo, TypeInfo> Cache = new Cache<TypeInfo, TypeInfo>( info => Type.GetType( $"{info.FullName}Metadata, {info.Assembly.FullName}", false )?.GetTypeInfo() );

			readonly TypeInfo current;

			public Context( TypeInfo current ) : this( current, Cache.Get( current ) ) {}

			Context( TypeInfo current, [Optional]TypeInfo metadata )
			{
				this.current = current;
				Metadata = metadata;
			}

			public Context CreateFromBaseType() => current.BaseType.With( x => new Context( x.GetTypeInfo() ) );

			public TypeInfo Metadata { get; }
		}
	}
}