using DragonSpark.Extensions;
using Ploeh.AutoFixture.Kernel;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing.Framework.Setup
{
	public class DefaultEngineParts : Ploeh.AutoFixture.DefaultEngineParts
	{
		readonly ISpecimenBuilderTransformation[] transformers;

		public static ISpecimenBuilderTransformation[] Default { get; } = { OptionalParameterTransformer.Instance };

		public static DefaultEngineParts Instance { get; } = new DefaultEngineParts();

		public DefaultEngineParts() : this( Default )
		{}

		public DefaultEngineParts( IEnumerable<ISpecimenBuilderTransformation> transformers )
		{
			this.transformers = transformers.Fixed();
		}

		public override IEnumerator<ISpecimenBuilder> GetEnumerator()
		{
			var enumerator = base.GetEnumerator();
			while ( enumerator.MoveNext() )
			{
				yield return transformers.Select( transformation => transformation.Transform( enumerator.Current ) ).NotNull().Only() ?? enumerator.Current;
			}
		}
	}
}