using System.Collections.Generic;
using System.Linq;
using DragonSpark.Runtime;

namespace DragonSpark
{
	public class PriorityAwareCollection<T> : CollectionBase<T> where T : IPriorityAware
	{
		public PriorityAwareCollection() {}
		public PriorityAwareCollection( IEnumerable<T> items ) : base( items ) {}
		public PriorityAwareCollection( ICollection<T> source ) : base( source ) {}

		protected override IEnumerable<T> Query => base.Query.OrderBy( arg => arg, PriorityComparer.Default );
	}
}