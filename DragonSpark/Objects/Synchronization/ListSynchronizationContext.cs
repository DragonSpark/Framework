using System;
using System.Collections;
using System.Linq;

namespace DragonSpark.Objects.Synchronization
{
	public class ListSynchronizationContext : ObjectResolvingSynchronizationContext
	{
		public ListSynchronizationContext( IObjectResolver resolver, string firstExpression, string secondExpression ) : base( resolver, firstExpression, secondExpression )
		{}

		public ListSynchronizationContext( IObjectResolver resolver, string firstExpression, string secondExpression, string mappingName ) : base( resolver, firstExpression, secondExpression, mappingName )
		{}

		public ListSynchronizationContext( IObjectResolver resolver, string firstExpression, string secondExpression, string mappingName, bool includeBasePolicies ) : base( resolver, firstExpression, secondExpression, mappingName, includeBasePolicies )
		{}

		public override void Synchronize(SynchronizationContainerContext context)
		{
			var propertyContext = ResolveInformation( context );
			var targetList = ResolveCollection( propertyContext.Target.Value );
			var type = ListSupport.ResolveElementType( targetList.GetType() );
			var source = ResolveCollection( propertyContext.Source.Value );
			var sourceList = EnsureCollection( propertyContext.Context.Container, source, type );
			ListSupport.Synchronize( sourceList, targetList );
		}

		protected override ISynchronizationContext CreateMirror()
		{
			var result = new ListSynchronizationContext( Resolver, SecondExpression, FirstExpression, MappingName, IncludeBasePolicies );
			return result;
		}

		IList EnsureCollection( SynchronizationContainer container, IList source, Type type )
		{
			var sourceType = ListSupport.ResolveElementType( source.GetType() );
			var result = !type.IsAssignableFrom( sourceType ) ? ConvertCollection( container, source, type ) : source;
			return result;
		}

		IList ConvertCollection( SynchronizationContainer container, IEnumerable source, Type type )
		{
			var query = from item in source.Cast<object>() 
						let target = ResolveTarget( container, item, type )
						where target != null
						select target;

			var result = query.ToList();
			return result;
		}

		static IList ResolveCollection( object target )
		{
			var source = ListSupport.ResolveList( target );
			if ( source != null )
			{
				return source;
			}
			throw new InvalidOperationException( string.Format( "Could not resolve a collection from target of type '{0}'",
			                                                    target.GetType() ) );
		}
	}
}