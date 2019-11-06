using AutoFixture.Kernel;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class DefaultTransformations : ArrayInstance<ISpecimenBuilderTransformation>
	{
		public static DefaultTransformations Default { get; } = new DefaultTransformations();

		DefaultTransformations() : base(OptionalParameterAlteration.Default, GreedyConstructorAlteration.Default) {}
	}
}