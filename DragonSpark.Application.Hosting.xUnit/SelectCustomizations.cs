using AutoFixture;
using AutoFixture.Kernel;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Hosting.xUnit;

sealed class SelectCustomizations : Select<IFixture, IList<ISpecimenBuilder>>
{
	public static SelectCustomizations Default { get; } = new();

	SelectCustomizations() : base(x => x.Customizations) {}
}