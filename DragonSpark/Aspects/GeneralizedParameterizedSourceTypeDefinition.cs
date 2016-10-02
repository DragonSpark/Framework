using DragonSpark.Aspects.Build;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Aspects
{
	public sealed class GeneralizedParameterizedSourceTypeDefinition : TypeDefinitionWithPrimaryMethodBase
	{
		public static GeneralizedParameterizedSourceTypeDefinition Default { get; } = new GeneralizedParameterizedSourceTypeDefinition();
		GeneralizedParameterizedSourceTypeDefinition() : base( new MethodStore( typeof(IParameterizedSource<object, object>), nameof(IParameterizedSource<object, object>.Get) ) ) {}
	}
}