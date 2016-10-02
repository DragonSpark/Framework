using System;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem.Metadata;

namespace DragonSpark
{
	public class PriorityAwareLocator<T> : ParameterizedSourceBase<T, IPriorityAware>
	{
		public static PriorityAwareLocator<T> Default { get; } = new PriorityAwareLocator<T>();
		PriorityAwareLocator() : this( o => AttributeSupport<PriorityAttribute>.Local.Get( o.GetType() ) ) {}

		readonly Func<T, IPriorityAware> get;

		protected PriorityAwareLocator( Func<T, IPriorityAware> get )
		{
			this.get = get;
		}

		public override IPriorityAware Get( T parameter ) => parameter as IPriorityAware ?? get( parameter ) ?? AssociatedPriority.Default.Get( parameter ) ?? DefaultPriorityAware.Default;
	}
}