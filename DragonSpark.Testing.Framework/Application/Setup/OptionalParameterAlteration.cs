using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public class OptionalParameterAlteration : EnginePartFactory<Ploeh.AutoFixture.Kernel.ParameterRequestRelay>
	{
		public static OptionalParameterAlteration Default { get; } = new OptionalParameterAlteration();
		OptionalParameterAlteration() {}

		public override ISpecimenBuilder Get( Ploeh.AutoFixture.Kernel.ParameterRequestRelay parameter ) => new ParameterRequestRelay( parameter );
	}
}