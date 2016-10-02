using DragonSpark.Commands;

namespace DragonSpark.Aspects.Validation
{
	sealed class GenericCommandAdapterFactory : AdapterFactorySource<IParameterValidationAdapter>
	{
		public static GenericCommandAdapterFactory Default { get; } = new GenericCommandAdapterFactory();
		GenericCommandAdapterFactory() : base( typeof(ICommand<>), typeof(CommandAdapter<>) ) {}
	}
}