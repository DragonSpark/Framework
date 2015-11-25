using Ploeh.AutoFixture.Kernel;
using System.Collections.Generic;

namespace DragonSpark.Testing.Framework
{
	public class DefaultEngineParts : Ploeh.AutoFixture.DefaultEngineParts
	{
		readonly MethodInvoker invoker = new MethodInvoker( new CompositeMethodQuery( new GreedyConstructorQuery(), new FactoryMethodQuery() ) );

		public static DefaultEngineParts Instance { get; } = new DefaultEngineParts();

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