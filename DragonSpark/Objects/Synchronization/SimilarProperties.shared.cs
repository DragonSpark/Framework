using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Objects.Synchronization
{
	/*public class CompositeExpressionContainer : ISynchronizationExpressionResolver
	{
		readonly IEnumerable<ISynchronizationExpressionResolver> resolvers;

		public CompositeExpressionContainer( IEnumerable<ISynchronizationExpressionResolver> resolvers )
		{
			this.resolvers = resolvers;
		}

		IEnumerable<SynchronizationContext> ResolveExpressions()
		{
			var query = from provider in resolvers
				            from expression in provider.Expressions
				            select expression;
			var result = query.ToArray();
			return result;
		}

		public ISynchronizationExpressionResolver CreateMirror()
		{
			var items = from provider in resolvers select provider.CreateMirror();
			var result = new CompositeExpressionContainer( items );
			return result;
		}

		#region Implementation of ISynchronizationExpressionResolver
		public IEnumerable<ISynchronizationContext> Resolve( SynchronizationKey key )
		{
			throw new NotImplementedException();
		}
		#endregion
	}*/

	public class SimilarProperties : ISynchronizationExpressionResolver
	{
		readonly string contextExpressionFirst;
		readonly string contextExpressionSecond;
		readonly IEnumerable<string> propertiesToIgnore;
		readonly bool includeNonPublicProperties;

		public SimilarProperties() : this( Enumerable.Empty<string>() )
		{}

		public SimilarProperties( string contextExpressionFirst, string contextExpressionSecond ) : this( contextExpressionFirst, contextExpressionSecond, Enumerable.Empty<string>(), false )
		{}

		public SimilarProperties( IEnumerable<string> propertiesToIgnore ) : this( null, null, propertiesToIgnore, false )
		{}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "NonPublic", Justification = "It is what it is." )]
		public SimilarProperties( string contextExpressionFirst, string contextExpressionSecond, IEnumerable<string> propertiesToIgnore, bool includeNonPublicProperties )
		{
			this.contextExpressionFirst = contextExpressionFirst;
			this.contextExpressionSecond = contextExpressionSecond;
			this.propertiesToIgnore = propertiesToIgnore;
			this.includeNonPublicProperties = includeNonPublicProperties;
		}

		#region Implementation of ISynchronizationExpressionResolver
		public IEnumerable<ISynchronizationContext> Resolve( SynchronizationKey key )
		{
			var hasFirst = !string.IsNullOrEmpty( contextExpressionFirst );
            var hasSecond = !string.IsNullOrEmpty( contextExpressionSecond );

			var targetKey =
				new SynchronizationKey(
					hasFirst ? Expression.Evaluate( key.First, contextExpressionFirst, false ).Last.Value.Property.PropertyType : key.First,
					hasSecond ? Expression.Evaluate( key.Second, contextExpressionSecond, false ).Last.Value.Property.PropertyType : key.Second
					);

			var contexts = from name in SimilarPropertyHelper.ResolveSimilarProperties( targetKey, includeNonPublicProperties )
                           where !propertiesToIgnore.Contains( name )
                           let firstExpression = hasFirst ? string.Format( "{0}.{1}", contextExpressionFirst, name ) : name
                           let secondExpression = hasSecond ? string.Format( "{0}.{1}", contextExpressionSecond, name ) : name
                           select new SynchronizationContext( firstExpression, secondExpression );
			var result = contexts.Cast<ISynchronizationContext>();
			return result;
		}
		#endregion
	}
}