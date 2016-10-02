using DragonSpark.Coercion;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	sealed class TypeDefinitions : ParameterizedScope<TypeInfo>
	{
		public static TypeDefinitions Default { get; } = new TypeDefinitions();
		TypeDefinitions() : base( new Factory().ToSourceDelegate().GlobalCache() ) {}

		sealed class Factory : DecoratedParameterizedSource<object, TypeInfo>
		{
			readonly static IParameterizedSource<object, TypeInfo>[]
				Factories = { TypeInfoDefinitionProvider.DefaultNested, MemberInfoDefinitionProvider.DefaultNested, GeneralDefinitionProvider.DefaultNested };

			public Factory() : this( ComponentModel.TypeDefinitions.Default.Get ) { }
			Factory( Alter<TypeInfo> alter ) : base( new CompositeFactory<object, TypeInfo>( Factories ).Apply( alter ) ) {}

			abstract class TypeDefinitionProviderBase<T> : ParameterizedSourceBase<T, TypeInfo> {}

			sealed class TypeInfoDefinitionProvider : TypeDefinitionProviderBase<TypeInfo>
			{
				public static IParameterizedSource<object, TypeInfo> DefaultNested { get; } = new TypeInfoDefinitionProvider().Apply( Common<TypeInfo>.Assigned ).Apply( Coercer<TypeInfo>.Default );
				TypeInfoDefinitionProvider() {}

				public override TypeInfo Get( TypeInfo parameter ) => parameter;
			}

			sealed class MemberInfoDefinitionProvider : TypeDefinitionProviderBase<MemberInfo>
			{
				public static IParameterizedSource<object, TypeInfo> DefaultNested { get; } = new MemberInfoDefinitionProvider().Apply( Common<MemberInfo>.Assigned ).Apply( Coercer<MemberInfo>.Default );
				MemberInfoDefinitionProvider() {}

				public override TypeInfo Get( MemberInfo parameter ) => parameter.DeclaringType.GetTypeInfo();
			}

			sealed class GeneralDefinitionProvider : TypeDefinitionProviderBase<object>
			{
				public static GeneralDefinitionProvider DefaultNested { get; } = new GeneralDefinitionProvider();
				GeneralDefinitionProvider() {}

				public override TypeInfo Get( object parameter ) => parameter.GetType().GetTypeInfo();
			}
		}
	}
}