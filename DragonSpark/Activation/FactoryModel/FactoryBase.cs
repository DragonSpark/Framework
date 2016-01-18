using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Setup.Registration;

namespace DragonSpark.Activation.FactoryModel
{
	public abstract class TransformerBase<T> : FactoryBase<T, T>, ITransformer<T>
	{}

	public abstract class FactoryBase<TParameter, TResult> : IFactory<TParameter, TResult>
	{
		readonly IFactoryParameterCoercer<TParameter> coercer;

		protected FactoryBase() : this( FixedFactoryParameterCoercer<TParameter>.Instance )
		{}

		protected FactoryBase( [Required]IFactoryParameterCoercer<TParameter> coercer )
		{
			this.coercer = coercer;
		}

		protected abstract TResult CreateItem( [Required]TParameter parameter );

		public TResult Create( TParameter parameter ) => CreateItem( parameter );

		object IFactoryWithParameter.Create( object parameter )
		{
			var qualified = coercer.Coerce( parameter );
			var result = Create( qualified );
			return result;
		}
	}

	public class SpecificationAwareFactory<T, U> : FactoryBase<T, U>
	{
		readonly ISpecification specification;
		readonly Func<T, U> inner;

		public SpecificationAwareFactory( Func<T, U> inner ) : this( AlwaysSpecification.Instance, inner ) {}

		public SpecificationAwareFactory( [Required]ISpecification<T> specification, [Required]Func<T, U> inner ) : this( (ISpecification)specification, inner ) {}

		SpecificationAwareFactory( [Required]ISpecification specification, [Required]Func<T, U> inner ) : base( FactoryParameterCoercer<T>.Instance )
		{
			this.specification = specification;
			this.inner = inner;
		}

		protected override U CreateItem( T parameter ) => specification.IsSatisfiedBy( parameter ) ? inner( (T)parameter ) : default(U);
	}

	public class FirstFromParameterFactory<T> : FirstFromParameterFactory<object, T>
	{
		public FirstFromParameterFactory( params IFactory<object, T>[] factories ) : base( factories ) {}

		public FirstFromParameterFactory( params Func<object, T>[] inner ) : base( inner ) {}
	}

	public class FirstFromParameterFactory<T, U> : FactoryBase<T, U>
	{
		readonly IEnumerable<Func<T, U>> inner;

		public FirstFromParameterFactory( params IFactory<T, U>[] factories ) : this( factories.Select( factory => factory.ToDelegate() ).ToArray() ) {}

		public FirstFromParameterFactory( [Required]params Func<T, U>[] inner ) : base( FactoryParameterCoercer<T>.Instance )
		{
			this.inner = inner;
		}

		protected override U CreateItem( T parameter )
		{
			var result = inner.FirstWhere( factory => factory( parameter ) );
			return result;
		}
	}

	public class FirstFactory<T> : FactoryBase<T>
	{
		readonly IEnumerable<Func<T>> inner;

		public FirstFactory( params IFactory<T>[] factories ) : this( factories.Select( factory => factory.ToDelegate() ).ToArray() ) { }

		public FirstFactory( [Required]params Func<T>[] inner )
		{
			this.inner = inner;
		}

		protected override T CreateItem() => inner.FirstWhere( factory => factory() );
	}

	public class AggregateFactory<T> : FactoryBase<T>
	{
		readonly Func<T> primary;
		readonly IEnumerable<Func<T, T>> transformers;

		public AggregateFactory( [Required]IFactory<T> primary, [Required]params ITransformer<T>[] transformers ) : this( primary.Create, transformers.Select( factory => factory.ToDelegate() ).ToArray() )
		{ }

		public AggregateFactory( [Required]Func<T> primary, [Required]params Func<T, T>[] transformers )
		{
			this.primary = primary;
			this.transformers = transformers;
		}

		protected override T CreateItem() => transformers.Aggregate( primary(), ( item, transformer ) => transformer( item ) );
	}

	public abstract class FactoryBase<TResult> : IFactory<TResult>
	{
		protected abstract TResult CreateItem();

		public TResult Create() => CreateItem();

		object IFactory.Create() => Create();
	}
}