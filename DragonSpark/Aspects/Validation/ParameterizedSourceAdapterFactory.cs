using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;

namespace DragonSpark.Aspects.Validation
{
	public sealed class ParameterizedSourceAdapterFactory : AdapterFactorySource<IParameterValidationAdapter>
	{
		public static ParameterizedSourceAdapterFactory Default { get; } = new ParameterizedSourceAdapterFactory();
		ParameterizedSourceAdapterFactory() : base( typeof(ISpecification<>), typeof(IParameterizedSource<,>), typeof(SourceAdapter<,>) ) {}
	}
}