namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class OptionalParameterAlteration : BuilderSelection<AutoFixture.Kernel.ParameterRequestRelay>
	{
		public static OptionalParameterAlteration Default { get; } = new OptionalParameterAlteration();

		OptionalParameterAlteration() : base(relay => new ParameterRequestRelay(relay)) {}
	}
}