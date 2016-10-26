using DragonSpark.Aspects.Build;

namespace DragonSpark.Aspects.Alteration
{
	sealed class Support<T> : SupportDefinition<T> where T : AspectBase
	{
		public static Support<T> Default { get; } = new Support<T>();
		Support() : base( GenericCommandCoreTypeDefinition.Default, ParameterizedSourceTypeDefinition.Default ) {}
	}
}