using DragonSpark.Aspects.Alteration;
using DragonSpark.Sources.Coercion;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using DragonSpark.Specifications;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	[ApplyResultAlteration( typeof(ComponentModel.TypeDefinitions) )]
	sealed class TypeDefinitions : CompositeFactory<object, TypeInfo>
	{
		readonly static IParameterizedSource<object, TypeInfo>[] Factories = { TypeInfoDefinitionProvider.Implementation, MemberInfoDefinitionProvider.Implementation, GeneralDefinitionProvider.Implementation };

		public static IParameterizedSource<object, TypeInfo> Default { get; } = new TypeDefinitions().ToSingletonScope();
		TypeDefinitions() : base( Factories ) {}
		
		abstract class TypeDefinitionProviderBase<T> : ParameterizedSourceBase<T, TypeInfo> {}

		sealed class TypeInfoDefinitionProvider : TypeDefinitionProviderBase<TypeInfo>
		{
			public static IParameterizedSource<object, TypeInfo> Implementation { get; } = Sources.Coercion.Extensions.Accept( new TypeInfoDefinitionProvider().Apply( Common<TypeInfo>.Assigned ), Coercer<TypeInfo>.Default );
			TypeInfoDefinitionProvider() {}

			public override TypeInfo Get( TypeInfo parameter ) => parameter;
		}

		sealed class MemberInfoDefinitionProvider : TypeDefinitionProviderBase<MemberInfo>
		{
			public static IParameterizedSource<object, TypeInfo> Implementation { get; } = Sources.Coercion.Extensions.Accept( new MemberInfoDefinitionProvider().Apply( Common<MemberInfo>.Assigned ), Coercer<MemberInfo>.Default );
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