using DragonSpark.Activation.FactoryModel;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Linq;

namespace DragonSpark.Testing.Framework.Setup
{
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

		protected override ISpecimenBuilder CreateItem( ISpecimenBuilder parameter )
		{
			var result = new CompositeSpecimenBuilder( builder, new FilteringSpecimenBuilder( parameter, any ) );
			return result;
		}
	}
}