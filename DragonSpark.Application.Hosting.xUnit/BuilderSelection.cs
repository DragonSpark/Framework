using AutoFixture;
using AutoFixture.Kernel;
using DragonSpark.Compose;
using System;

namespace DragonSpark.Application.Hosting.xUnit
{
	public class BuilderSelection<T> : ISpecimenBuilderTransformation where T : ISpecimenBuilder
	{
		BuilderSelection(Func<T, ISpecimenBuilderNode> @delegate) => _delegate = @delegate;

		readonly Func<T, ISpecimenBuilderNode> _delegate;

		protected BuilderSelection(Func<T, ISpecimenBuilder> @delegate)
			: this(@delegate.Start().Select(Start.An.Extent<CustomizationNode>().New)) {}

		public ISpecimenBuilderNode Transform(ISpecimenBuilder builder) => builder.AsTo(_delegate);
	}
}