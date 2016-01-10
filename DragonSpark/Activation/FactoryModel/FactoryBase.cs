using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

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

	public class FactoryWithSpecification<T> : FactoryBase<object, T>
	{
		readonly ISpecification specification;
		readonly Func<object, T> inner;

		public FactoryWithSpecification( [Required]ISpecification specification, [Required]Func<object, T> inner  )
		{
			this.specification = specification;
			this.inner = inner;
		}

		protected override T CreateItem( object parameter ) => specification.IsSatisfiedBy( parameter ) ? inner( parameter ) : default(T);
	}

	public class FirstFactory<T, U> : FactoryBase<T, U>
	{
		readonly IEnumerable<IFactory<T, U>> inner;

		public FirstFactory( [Required]params IFactory<T, U>[] inner )
		{
			this.inner = inner;
		}

		protected override U CreateItem( T parameter ) => inner.FirstWhere( factory => factory.Create( parameter ) );
	}

	public class FirstFactory<T> : FactoryBase<T>
	{
		readonly IEnumerable<IFactory<T>> inner;

		public FirstFactory( [Required]params IFactory<T>[] inner )
		{
			this.inner = inner;
		}

		protected override T CreateItem() => inner.FirstWhere( factory => factory.Create() );
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