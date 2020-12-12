using AutoFixture;
using AutoFixture.Kernel;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Hosting.xUnit
{
	public class AutoDataAttribute : AutoFixture.Xunit2.AutoDataAttribute
	{
		public AutoDataAttribute() : this(Fixtures.Default) {}

		protected AutoDataAttribute(params ISpecimenBuilder[] specimens)
			: this(specimens.AsValueEnumerable()
			                .Select(x => (ICustomization)new InsertCustomization(x))
			                .ToArray()) {}

		protected AutoDataAttribute(params ICustomization[] customizations)
			: this(new DefaultCustomizations(customizations)) {}

		protected AutoDataAttribute(params ISpecimenBuilderTransformation[] transformations)
			: this(DefaultCustomizations.Default, transformations) {}

		protected AutoDataAttribute(ICustomization customization,
		                            params ISpecimenBuilderTransformation[] transformations)
			: this(transformations, customization) {}

		protected AutoDataAttribute(IEnumerable<ISpecimenBuilderTransformation> transformations,
		                            ICustomization customization)
			: this(new Fixtures(new EngineParts(transformations.Open()), customization)) {}

		protected AutoDataAttribute(IResult<IFixture> result) : base(result.Get) {}

		[UsedImplicitly]
		protected AutoDataAttribute(Func<IFixture> fixture) : base(fixture) {}
	}
}