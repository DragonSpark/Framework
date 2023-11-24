using AutoFixture.Kernel;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Hosting.xUnit;

sealed class NoSpecimenInstance : Instance<NoSpecimen>
{
	public static NoSpecimenInstance Default { get; } = new();

	NoSpecimenInstance() : base(new NoSpecimen()) {}
}