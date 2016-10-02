using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.TypeSystem.Generics
{
	sealed class GenericMethodContext<T> : ArgumentCache<Type[], MethodContext<T>>, IGenericMethodContext<T> where T : class
	{
		public GenericMethodContext( IEnumerable<Descriptor> descriptors, Func<MethodInfo, T> create ) : this( new Factory( descriptors, create ).Get ) {}
		GenericMethodContext( Func<Type[], MethodContext<T>> resultSelector ) : base( resultSelector ) {}

		public MethodContext<T> Make( params Type[] types ) => Get( types );

		sealed class Factory : ParameterizedSourceBase<Type[], MethodContext<T>>
		{
			readonly Func<MethodInfo, T> create;
			readonly Func<ValueTuple<Descriptor, Type[]>, GenericMethodCandidate<T>> selector;

			readonly ImmutableArray<Descriptor> descriptors;

			public Factory( IEnumerable<Descriptor> descriptors, Func<MethodInfo, T> create )
			{
				this.create = create;
				this.descriptors = descriptors.ToImmutableArray();
				selector = CreateSelector;
			}

			GenericMethodCandidate<T> CreateSelector( ValueTuple<Descriptor, Type[]> item )
			{
				try
				{
					var method = item.Item1.Method.MakeGenericMethod( item.Item2 );
					var specification = CompatibleArgumentsSpecification.Default.Get( method ).ToSpecificationDelegate();
					var @delegate = create( method );
					var result = new GenericMethodCandidate<T>( @delegate, specification );
					return result;
				}
				catch ( ArgumentException e )
				{
					Logger.Default.Get( this ).Verbose( e, "Could not create a generic method for {Method} with types {Types}", item.Item1.Method, item.Item2 );
					return default(GenericMethodCandidate<T>);
				}
			}

			public override MethodContext<T> Get( Type[] parameter )
			{
				var candidates = descriptors.Introduce( parameter, tuple => tuple.Item1.Specification( tuple.Item2 ), selector ).WhereAssigned().ToImmutableArray();
				var result = new MethodContext<T>( candidates );
				return result;
			}
		}
	}
}