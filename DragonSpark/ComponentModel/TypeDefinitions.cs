using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public sealed class TypeDefinitions : AlterationBase<TypeInfo>, ITypeDefinitionProvider
	{
		public static IParameterizedSource<TypeInfo, TypeInfo> Default { get; } = new TypeDefinitions();
		TypeDefinitions() : this( Source.Implementation.Get ) {}

		readonly Func<ImmutableArray<ITypeDefinitionProvider>> source;

		[UsedImplicitly]
		public TypeDefinitions( Func<ImmutableArray<ITypeDefinitionProvider>> source )
		{
			this.source = source;
		}

		public override TypeInfo Get( TypeInfo parameter )
		{
			foreach ( var provider in source() )
			{
				var info = provider.Get( parameter );
				if ( info != null )
				{
					return info;
				}
			}
			return null;
		}

		public sealed class Source : Scope<ImmutableArray<ITypeDefinitionProvider>>
		{
			public static Source Implementation { get; } = new Source();
			Source() : base( () => ImmutableArray.Create<ITypeDefinitionProvider>( ConventionTypeDefinitionProvider.Default, Self.Instance ) ) {}
			
			sealed class Self : SelfAlteration<TypeInfo>, ITypeDefinitionProvider
			{
				public static Self Instance { get; } = new Self();
				Self() {}
			}
		}
	}
}