using System.Collections.Generic;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework
{
	public class GreedyEngineParts : DefaultEngineParts
	{
		readonly MethodInvoker invoker = new MethodInvoker( new CompositeMethodQuery( new GreedyConstructorQuery(), new FactoryMethodQuery() ) );

		public static GreedyEngineParts Instance { get; } = new GreedyEngineParts();

		public override IEnumerator<ISpecimenBuilder> GetEnumerator()
		{
			var iter = base.GetEnumerator();
			while ( iter.MoveNext() )
			{
				var builder = iter.Current is MethodInvoker ? invoker : iter.Current;
				yield return builder;
			}
		}
	}
}