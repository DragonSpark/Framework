using DragonSpark.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Runtime
{
	public interface ISpecification
	{
		bool IsSatisfiedBy( object context );
	}

	public class ProvidedSpecification : ISpecification
	{
		readonly Func<bool> resolver;

		public ProvidedSpecification( Func<bool> resolver )
		{
			this.resolver = resolver;
		}

		public bool IsSatisfiedBy( object context )
		{
			var result = resolver();
			return result;
		}
	}

	public abstract class CompositeSpecification : ISpecification
	{
		readonly Func<Func<ISpecification, bool>, bool> @where;
		
		protected CompositeSpecification( Func<Func<ISpecification, bool>, bool> where )
		{
			this.@where = @where;
		}

		public bool IsSatisfiedBy( object context )
		{
			var result = where( condition => condition.IsSatisfiedBy( context ) );
			return result;
		}
	}

	public class AnySpecification : CompositeSpecification
	{
		public AnySpecification( params ISpecification[] specifications ) : base( specifications.Any )
		{}
	}

	public class AllSpecification : CompositeSpecification
	{
		public AllSpecification( params ISpecification[] specifications ) : base( specifications.All )
		{}
	}

	public abstract class SpecificationBase<TContext> : ISpecification
	{
		protected SpecificationBase( TContext context )
		{
			Context = context;
		}

		protected TContext Context { get; }

		bool ISpecification.IsSatisfiedBy( object context )
		{
			return IsSatisfiedBy( context );
		}


		protected abstract bool IsSatisfiedBy( object context );
	}

	public abstract class Specification<TContext> : SpecificationBase<TContext>
	{
		protected Specification( TContext context ) : base( context )
		{}

		protected override bool IsSatisfiedBy( object context )
		{
			var result = context is TContext && IsSatisfiedByContext( (TContext)context );
			return result;
		}

		protected abstract bool IsSatisfiedByContext( TContext context );
	}

	public class EqualitySpecification : Specification<object>
	{
		public EqualitySpecification( object context ) : base( context )
		{}
		protected override bool IsSatisfiedByContext( object context )
		{
			var result = Equals( Context, context );
			return result;
		}
	}

	public class MemberInfoSpecification : AnySpecification
	{
		public MemberInfoSpecification( MemberInfo member ) : base( new EqualitySpecification( member ), new TypeSpecification( member.DeclaringType ) )
		{}
	}

	public class TypeSpecification : Specification<Type>
	{
		public TypeSpecification( Type targetType ) : base( targetType )
		{}

		protected override bool IsSatisfiedByContext( Type context )
		{
			var result = Context.Extend().IsAssignableFrom( context );
			return result;
		}
	}
}