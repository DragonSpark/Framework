using DragonSpark.Aspects.Alteration;
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
		TypeDefinitions() : base( new Factory().ToDelegate().GlobalCache() ) {}

		[ApplyResultAlteration( typeof(ComponentModel.TypeDefinitions) )]
		sealed class Factory : CompositeFactory<object, TypeInfo>
		{
			readonly static IParameterizedSource<object, TypeInfo>[] Factories = { TypeInfoDefinitionProvider.Implementation, MemberInfoDefinitionProvider.Implementation, GeneralDefinitionProvider.Implementation };

			public Factory() : base( Factories ) {}

			abstract class TypeDefinitionProviderBase<T> : ParameterizedSourceBase<T, TypeInfo> {}

			sealed class TypeInfoDefinitionProvider : TypeDefinitionProviderBase<TypeInfo>
			{
				public static IParameterizedSource<object, TypeInfo> Implementation { get; } = new TypeInfoDefinitionProvider().Apply( Common<TypeInfo>.Assigned ).Apply( Coercer<TypeInfo>.Default );
				TypeInfoDefinitionProvider() {}

				public override TypeInfo Get( TypeInfo parameter ) => parameter;
			}

			sealed class MemberInfoDefinitionProvider : TypeDefinitionProviderBase<MemberInfo>
			{
				public static IParameterizedSource<object, TypeInfo> Implementation { get; } = new MemberInfoDefinitionProvider().Apply( Common<MemberInfo>.Assigned ).Apply( Coercer<MemberInfo>.Default );
				MemberInfoDefinitionProvider() {}

				public override TypeInfo Get( MemberInfo parameter ) => parameter.DeclaringType.GetTypeInfo();
			}

			sealed class GeneralDefinitionProvider : TypeDefinitionProviderBase<object>
			{
				public static GeneralDefinitionProvider Implementation { get; } = new GeneralDefinitionProvider();
				GeneralDefinitionProvider() {}

				public override TypeInfo Get( object parameter ) => parameter.GetType().GetTypeInfo();
			}
		}
	}
}