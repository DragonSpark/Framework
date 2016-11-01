using System;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public abstract class EnginePartFactory<T> : ParameterizedSourceBase<T, ISpecimenBuilder>, ISpecimenBuilderTransformation where T : ISpecimenBuilder
	{
		readonly Func<T, ISpecimenBuilder> toDelegate;

		protected EnginePartFactory()
		{
			toDelegate = this.ToDelegate();
		}

		public ISpecimenBuilder Transform( ISpecimenBuilder builder ) => builder.AsTo( toDelegate );
	}
}