using AutoFixture;
using AutoFixture.Kernel;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Hosting.xUnit
{
	public static class Extensions
	{
		public static ISpecimenBuilder AsSpecimen<T>(this IResult<T> @this) => new Specimen<T>(@this.Get);

		public static ICustomization AsCustomization<T>(this IResult<T> @this)
			=> new InsertCustomization(@this.AsSpecimen());
	}
}
