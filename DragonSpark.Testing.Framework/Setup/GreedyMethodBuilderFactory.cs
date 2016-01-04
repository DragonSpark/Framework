using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Testing.Framework.Setup
{
	public abstract class EnginePartFactory<T> : FactoryBase<T, ISpecimenBuilder>, ISpecimenBuilderTransformation where T : ISpecimenBuilder
	{
		public ISpecimenBuilder Transform( ISpecimenBuilder builder ) => builder.AsTo<T, ISpecimenBuilder>( Create );
	}

	public class OptionalParameterTransformer : EnginePartFactory<Ploeh.AutoFixture.Kernel.ParameterRequestRelay>
	{
		public static OptionalParameterTransformer Instance { get; } = new OptionalParameterTransformer();

		protected override ISpecimenBuilder CreateItem( Ploeh.AutoFixture.Kernel.ParameterRequestRelay parameter ) => new ParameterRequestRelay( parameter );
	}

	public class ParameterRequestRelay : ISpecimenBuilder
	{
		readonly Ploeh.AutoFixture.Kernel.ParameterRequestRelay inner;

		public ParameterRequestRelay( [Required]Ploeh.AutoFixture.Kernel.ParameterRequestRelay inner )
		{
			this.inner = inner;
		}

		public object Create( object request, [Required]ISpecimenContext context ) => request.AsTo<ParameterInfo, object>( info => ShouldDefault( info, context ) ? info.DefaultValue : inner.Create( request, context ), () => new NoSpecimen() );

		static bool ShouldDefault( ParameterInfo info, ISpecimenContext context )
		{
			var result = info.IsOptional && !FixtureLocator.Instance.Create( context ).With( fixture => new AssociatedSetup( fixture ).Item.Method.GetParameters().Select( pi => pi.ParameterType ).Any( info.ParameterType.Adapt().IsAssignableFrom ) );
			return result;
		}
	}

	public class FixtureLocator : FactoryBase<ISpecimenContext, IFixture>
	{
		public static FixtureLocator Instance { get; } = new FixtureLocator();

		protected override IFixture CreateItem( ISpecimenContext parameter ) => parameter.AsTo<SpecimenContext, IFixture>( specimenContext => specimenContext.Builder as IFixture );
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