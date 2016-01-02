using Ploeh.AutoFixture.Kernel;
using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Testing.Framework.Setup
{
	public abstract class EnginePartFactory<T> : FactoryBase<T, ISpecimenBuilder>, ISpecimenBuilderTransformation where T : ISpecimenBuilder
	{
		public ISpecimenBuilder Transform( ISpecimenBuilder builder )
		{
			var result = builder.AsTo<T, ISpecimenBuilder>( Create );
			return result;
		}
	}

	public class OptionalParameterTransformer : EnginePartFactory<Ploeh.AutoFixture.Kernel.ParameterRequestRelay>
	{
		public static OptionalParameterTransformer Instance { get; } = new OptionalParameterTransformer();

		protected override ISpecimenBuilder CreateItem( Ploeh.AutoFixture.Kernel.ParameterRequestRelay parameter )
		{
			var result = new ParameterRequestRelay( parameter );
			return result;
		}
	}

	public class ParameterRequestRelay : ISpecimenBuilder
	{
		readonly Ploeh.AutoFixture.Kernel.ParameterRequestRelay inner;

		public ParameterRequestRelay( [Required]Ploeh.AutoFixture.Kernel.ParameterRequestRelay inner )
		{
			this.inner = inner;
		}

		public object Create( object request, [Required]ISpecimenContext context )
		{
			var result = request.AsTo<ParameterInfo, object>( info => info.IsOptional ? info.DefaultValue : inner.Create( request, context ) ) ?? new NoSpecimen();
			return result;
		}
	}

	public class GreedyMethodBuilderFactory : EnginePartFactory<MethodInvoker>
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

		protected override ISpecimenBuilder CreateItem( MethodInvoker parameter )
		{
			var result = new CompositeSpecimenBuilder( builder, new FilteringSpecimenBuilder( parameter, any ) );
			return result;
		}
	}
}