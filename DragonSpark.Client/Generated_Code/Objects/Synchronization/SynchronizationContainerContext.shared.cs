namespace DragonSpark.Objects.Synchronization
{
	public class SynchronizationContainerContext
	{
		readonly SynchronizationContainer container;
		readonly SynchronizationKey key;
		readonly object source;
		readonly object target;
		readonly bool includeBasePolicies;

		public SynchronizationContainerContext( SynchronizationContainer container, object source, object target ) : this( container, source, target, false )
		{}

		public SynchronizationContainerContext( SynchronizationContainer container, object source, object target, bool includeBasePolicies ) : this( container, source, target, null, includeBasePolicies )
		{}

		public SynchronizationContainerContext( SynchronizationContainer container, object source, object target, string name, bool includeBasePolicies ) : this( container, new SynchronizationKey( source.GetType(), target.GetType(), !string.IsNullOrEmpty( name ) ? name : null ), source, target, includeBasePolicies )
		{}

		public SynchronizationContainerContext( SynchronizationContainer container, SynchronizationKey key, object source, object target ) : this( container, key, source, target, false )
		{}

		public SynchronizationContainerContext( SynchronizationContainer container, SynchronizationKey key, object source, object target, bool includeBasePolicies )
		{
			this.container = container;
			this.key = key;
			this.source = source;
			this.target = target;
			this.includeBasePolicies = includeBasePolicies;
		}

		public SynchronizationContainer Container
		{
			get { return container; }
		}

		public bool IncludeBasePolicies
		{
			get { return includeBasePolicies; }
		}

		public object Target
		{
			get { return target; }
		}

		public object Source
		{
			get { return source; }
		}

		public SynchronizationKey Key
		{
			get { return key; }
		}
	}
}