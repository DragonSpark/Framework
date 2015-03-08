using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Objects.Synchronization
{
	public class SynchronizationContainer : IComparer<Type>
	{
		public SynchronizationContainer() : this( new List<ISynchronizationPolicy>() )
		{}

		public SynchronizationContainer( IEnumerable<ISynchronizationPolicy> policies )
		{
			Contract.Requires( policies != null );

			this.policies = new List<ISynchronizationPolicy>( policies );
			
			ContinueOnMappingException = true;
		}
		
		public bool ContinueOnMappingException { get; set; }

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( policies != null );
		}*/

		public IList<ISynchronizationPolicy> Policies
		{
			get { return policies; }
		}	readonly List<ISynchronizationPolicy> policies;

		public void Synchronize( object source, object target )
		{
			Synchronize( source, target, null );
		}

		public void Synchronize( object source, object target, string name )
		{
			Synchronize( source, target, name, false );
		}

		public void Synchronize( object source, object target, bool includeBasePolicies )
		{
			Synchronize( source, target, null, includeBasePolicies );
		}

		public void Synchronize( object source, object target, string name, bool includeBasePolicies )
		{
			var context = new SynchronizationContainerContext( this, source, target, name, includeBasePolicies );
			Synchronize( context );
		}

		public void Synchronize( SynchronizationContainerContext context )
		{
			var contexts = ResolveContexts( context.Key, context.IncludeBasePolicies );
			contexts.Apply( item => item.Synchronize( context ) );
		}

		IEnumerable<ISynchronizationContext> ResolveContexts( SynchronizationKey key, bool includeBasePolicies )
		{
			var items = from policy in Policies 
						select new
						       	{
						       		policy.Key, 
									Contexts = from resolver in policy.Resolvers
												  from context in resolver.Resolve( key ) 
												  select context
						       	};

			var normal =
				from item in items 
				where includeBasePolicies ? item.Key.IsAssignable( key  ) : item.Key == key
				from context in item.Contexts
				select context;

			var mirror = new SynchronizationKey( key.Second, key.First );
			var mirrored = from item in items 
						   where includeBasePolicies ? item.Key.IsAssignable( mirror ) : item.Key == mirror
						   from context in item.Contexts
						   select context.CreateMirror();

			var result = Filtered( normal ).Union( Filtered( mirrored ) ).Distinct();

			return result;
		}

		IEnumerable<ISynchronizationContext> Filtered( IEnumerable<ISynchronizationContext> items )
		{
			Contract.Assume( items != null );

			var result = items.NotNull().OrderBy( item => item.TypeConverterType, this );
			return result;
		}

		#region Implementation of IComparer<Type>
		public int Compare( Type x, Type y )
		{
			var result = x == y ? 0 : x != null || y != null ? 1 : 0;
			return result;
		}
		#endregion
	}
}
