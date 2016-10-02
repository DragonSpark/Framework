using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	sealed class MemberInfoDefinitions : ParameterizedScope<MemberInfo>
	{
		public static IParameterizedSource<MemberInfo> Default { get; } = new MemberInfoDefinitions();
		MemberInfoDefinitions() : base( new Factory( TypeDefinitions.Default.Get ).ToSourceDelegate().GlobalCache() ) {}

		sealed class Factory : ParameterizedSourceBase<MemberInfo>
		{
			readonly static ImmutableArray<Func<object, IMemberInfoSource>> Delegates = 
				new[] { typeof(PropertyInfoDefinitionLocator), typeof(ConstructorInfoDefinitionLocator), typeof(MethodInfoDefinitionLocator), typeof(TypeInfoDefinitionLocator) }
				.Select( type => ParameterConstructor<IMemberInfoSource>.Make( typeof(TypeInfo), type ) )
				.ToImmutableArray();
		
			readonly Func<object, TypeInfo> typeSource;

			public Factory( Func<object, TypeInfo> typeSource )
			{
				this.typeSource = typeSource;
			}

			public override MemberInfo Get( object parameter )
			{
				var definition = typeSource( parameter );
				foreach ( var @delegate in Delegates )
				{
					var source = @delegate( definition );
					var result = source.Get( parameter );
					if ( result != null )
					{
						return result;
					}
				}
				return null;
			}

			sealed class PropertyInfoDefinitionLocator : NamedMemberInfoDefinitionLocatorBase<PropertyInfo>
			{
				public PropertyInfoDefinitionLocator( TypeInfo definition ) : base( definition, definition.GetDeclaredProperty, definition.DeclaredProperties ) {}
			}

			sealed class ConstructorInfoDefinitionLocator : MemberInfoDefinitionLocatorBase<ConstructorInfo>
			{
				public ConstructorInfoDefinitionLocator( TypeInfo definition ) : base( definition ) {}
				protected override MemberInfo From( ConstructorInfo parameter ) => 
					Definition.DeclaredConstructors.Introduce( parameter.GetParameterTypes(), tuple => tuple.Item1.GetParameterTypes().SequenceEqual( tuple.Item2 ) ).FirstOrDefault();
			}

			sealed class MethodInfoDefinitionLocator : NamedMemberInfoDefinitionLocatorBase<MethodInfo>
			{
				public MethodInfoDefinitionLocator( TypeInfo definition ) : base( definition, definition.GetDeclaredMethod, definition.DeclaredMethods ) {}
			}

			sealed class TypeInfoDefinitionLocator : MemberInfoDefinitionLocatorBase<object>
			{
				public TypeInfoDefinitionLocator( TypeInfo definition ) : base( definition ) {}

				protected override MemberInfo From( object parameter ) => Definition;
			}

			abstract class NamedMemberInfoDefinitionLocatorBase<T> : MemberInfoDefinitionLocatorBase<T> where T : MemberInfo
			{
				readonly Func<string, T> source;
				readonly IEnumerable<T> all;

				protected NamedMemberInfoDefinitionLocatorBase( TypeInfo definition, Func<string, T> source, IEnumerable<T> all ) : base( definition )
				{
					this.source = source;
					this.all = all;
				}

				protected override MemberInfo From( T parameter )
				{
					try
					{
						return source( parameter.Name );
					}
					catch ( AmbiguousMatchException )
					{
						var result = all.Introduce( parameter, tuple => tuple.Item1.Name == tuple.Item2.Name ).FirstOrDefault();
						return result;
					}
				}
			}

			interface IMemberInfoSource : IParameterizedSource<object, MemberInfo> {}

			abstract class MemberInfoDefinitionLocatorBase<T> : ParameterizedSourceBase<T, MemberInfo>, IMemberInfoSource
			{
				protected MemberInfoDefinitionLocatorBase( TypeInfo definition )
				{
					Definition = definition;
				}

				protected TypeInfo Definition { get; }

				public override MemberInfo Get( T parameter ) => From( parameter ) ?? parameter as MemberInfo ?? Definition;

				protected abstract MemberInfo From( T parameter );
				MemberInfo IParameterizedSource<object, MemberInfo>.Get( object parameter ) => parameter is T ? Get( (T)parameter ) : null;
			}
		}
	}
}