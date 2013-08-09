using System;
using System.Collections.Generic;
// using System.Diagnostics.Contracts;
using System.Linq;
using DragonSpark;

namespace DragonSpark.Objects.Synchronization
{
	public class SynchronizationExpressionResolver : ISynchronizationExpressionResolver
	{
		readonly IEnumerable<ISynchronizationContext> expressions;

		public SynchronizationExpressionResolver( IEnumerable<ISynchronizationContext> expressions )
		{
			// Contract.Requires( expressions != null );
			this.expressions = expressions;
		}

		#region Implementation of ISynchronizationExpressionResolver
		public IEnumerable<ISynchronizationContext> Resolve( SynchronizationKey key )
		{
			return expressions;
		}
		#endregion

		/*public virtual IEnumerable<ISynchronizationContext> RetrieveExpressions()
		{
			var result = expressions.Cast<ISynchronizationContext>();
			return result;
		}

		protected IEnumerable<SynchronizationContext> CreateMirroredContexts()
		{
			var result = from context in expressions
			             select new SynchronizationContext( context.SecondExpression, context.FirstExpression,
			                                                              context.TypeConverterType );
			return result;
		}

		protected virtual ISynchronizationExpressionResolver CreateMirror()
		{
			var mirrored = CreateMirroredContexts();
			var result = new PropertyMappingContextProvider( mirrored );
			return result;
		}

		ISynchronizationExpressionResolver ISynchronizationExpressionResolver.CreateMirror()
		{
			var result = enableMirroring ? CreateMirror() : null;
			return result;
		}
*/
	}
}