using DragonSpark.Activation;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing.Framework
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

	public class GreedyMethodBuilderFactory : FactoryBase<ISpecimenBuilder, ISpecimenBuilder>
	{
		public static GreedyMethodBuilderFactory Instance { get; } = new GreedyMethodBuilderFactory();

		readonly Type[] exceptions = { typeof(DateTimeOffset) };
		readonly IRequestSpecification any;
		readonly ISpecimenBuilder greedy = new MethodInvoker( new CompositeMethodQuery( new GreedyConstructorQuery(), new FactoryMethodQuery() ) ), builder;

		public GreedyMethodBuilderFactory()
		{
			any = new OrRequestSpecification( exceptions.Select( type => new ExactTypeSpecification( type ) ) );
			builder = new FilteringSpecimenBuilder( greedy, new InverseRequestSpecification( any ) );
		}

		protected override ISpecimenBuilder CreateFrom( ISpecimenBuilder parameter )
		{
			var result = new CompositeSpecimenBuilder( builder, new FilteringSpecimenBuilder( parameter, any ) );
			return result;
		}
	}
}