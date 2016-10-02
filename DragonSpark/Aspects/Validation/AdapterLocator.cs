namespace DragonSpark.Aspects.Validation
{
	sealed class AdapterLocator : AdapterLocatorBase<IParameterValidationAdapter>
	{
		public static AdapterLocator Default { get; } = new AdapterLocator();
		AdapterLocator() : base( Defaults.Factories ) {}
	}
}