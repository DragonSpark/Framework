using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using PostSharp.Extensibility;
using System.Reflection;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.TypeSystem
{
	public abstract class AssemblyProviderBase : FactoryBase<Assembly[]>, IAssemblyProvider
	{
		[Cache( AttributeInheritance = MulticastInheritance.Multicast, AttributeTargetMemberAttributes = MulticastAttributes.Instance )]
		protected abstract override Assembly[] CreateItem();
	}

	public class AggregateAssemblyFactory : AggregateFactory<Assembly[]>, IAssemblyProvider
	{
		public AggregateAssemblyFactory( IFactory<Assembly[]> primary, params IFactory<Assembly[], Assembly[]>[] factories ) : base( primary, factories )
		{}

		public AggregateAssemblyFactory( Func<Assembly[]> primary, params Func<Assembly[], Assembly[]>[] transformers ) : base( primary, transformers )
		{}
	}

	public class AggregateFactory<T> : FactoryBase<T>
	{
		readonly Func<T> primary;
		readonly IEnumerable<Func<T, T>> transformers;

		public AggregateFactory( [Required]IFactory<T> primary, [Required]params IFactory<T, T>[] factories ) : this( primary.Create, factories.Select( factory => new Func<T, T>( factory.Create ) ).ToArray() )
		{}

		public AggregateFactory( [Required]Func<T> primary, [Required]params Func<T, T>[] transformers )
		{
			this.primary = primary;
			this.transformers = transformers;
		}

		protected override T CreateItem()
		{
			var seed = primary();
			var result = transformers.Aggregate( seed, ( item, transformer ) => transformer( item ) );
			return result;
		}
	}
}