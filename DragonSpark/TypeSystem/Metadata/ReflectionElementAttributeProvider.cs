using DragonSpark.Sources.Parameterized;

namespace DragonSpark.TypeSystem.Metadata
{
	sealed class ReflectionElementAttributeProvider : ParameterConstructedCompositeFactory<IAttributeProvider>
	{
		public static IParameterizedSource<IAttributeProvider> Default { get; } = new ReflectionElementAttributeProvider().ToCache();
		ReflectionElementAttributeProvider() : base( typeof(TypeInfoAttributeProvider), typeof(PropertyInfoAttributeProvider), typeof(MethodInfoAttributeProvider), typeof(MemberInfoAttributeProvider) ) {}
	}
}