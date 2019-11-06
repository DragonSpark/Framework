using AutoFixture.Kernel;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class GreedyConstructorAlteration : BuilderSelection<MethodInvoker>
	{
		public static GreedyConstructorAlteration Default { get; } = new GreedyConstructorAlteration();

		GreedyConstructorAlteration()
			: base(new MethodInvoker(new CompositeMethodQuery(new GreedyConstructorQuery(), new FactoryMethodQuery()))
				       .Accept) {}
	}
}