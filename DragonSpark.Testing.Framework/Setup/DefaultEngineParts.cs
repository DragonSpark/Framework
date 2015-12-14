using DragonSpark.Extensions;
using Ploeh.AutoFixture.Kernel;
using System.Collections.Generic;

namespace DragonSpark.Testing.Framework.Setup
{
	public class DefaultEngineParts : Ploeh.AutoFixture.DefaultEngineParts
	{
		public static DefaultEngineParts Instance { get; } = new DefaultEngineParts();

		public override IEnumerator<ISpecimenBuilder> GetEnumerator()
		{
			var enumerator = base.GetEnumerator();
			while ( enumerator.MoveNext() )
			{
				var builder = enumerator.Current.AsTo<MethodInvoker, ISpecimenBuilder>( GreedyMethodBuilderFactory.Instance.Create ) ?? enumerator.Current;
				yield return builder;
			}
		}
	}
}