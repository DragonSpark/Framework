using AutoFixture;
using AutoFixture.Kernel;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;
using System.Collections.Generic;

namespace DragonSpark.Application.Hosting.xUnit
{
	public sealed class EngineParts : DefaultEngineParts
	{
		public static EngineParts Default { get; } = new EngineParts();

		EngineParts() : this(DefaultTransformations.Default.Get().Open()) {}

		readonly IEnumerable<ISpecimenBuilderTransformation> _transformers;

		[UsedImplicitly]
		public EngineParts(IEnumerable<ISpecimenBuilderTransformation> transformers)
			=> _transformers = transformers;

		public override IEnumerator<ISpecimenBuilder> GetEnumerator()
		{
			using var enumerator = base.GetEnumerator();
			while (enumerator.MoveNext())
			{
				yield return Transform(enumerator.Current!);
			}
		}

		ISpecimenBuilder Transform(ISpecimenBuilder current)
		{
			foreach (var transformer in _transformers.AsValueEnumerable())
			{
				var transformed = transformer.Transform(current);
				if (transformed != null)
				{
					return transformed;
				}
			}

			return current;
		}
	}
}