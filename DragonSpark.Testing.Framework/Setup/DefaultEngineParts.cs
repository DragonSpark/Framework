using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework.Setup
{
	public class DefaultEngineParts : Ploeh.AutoFixture.DefaultEngineParts
	{
		public static DefaultEngineParts Instance { get; } = new DefaultEngineParts();

		public override IEnumerator<ISpecimenBuilder> GetEnumerator()
		{
			var iter = base.GetEnumerator();
			while ( iter.MoveNext() )
			{
				var builder = iter.Current is MethodInvoker ? GreedyMethodBuilderFactory.Instance.Create( iter.Current ) : iter.Current;
				yield return builder;
			}
		}
	}
}