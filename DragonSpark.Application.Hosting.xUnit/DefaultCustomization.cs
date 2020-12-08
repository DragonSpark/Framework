using AutoFixture;
using AutoFixture.AutoMoq;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class DefaultCustomization : CompositeCustomization
	{
		public static DefaultCustomization Default { get; } = new DefaultCustomization();

		DefaultCustomization() : base(ManualPropertyTypesCustomization.Default,
		                              SingletonCustomization.Default,
		                              new InsertCustomization(TimeSpecimen.Default),
		                              new InsertCustomization(EpochSpecimen.Default),
		                              new AutoMoqCustomization {ConfigureMembers = true}) {}
	}
}