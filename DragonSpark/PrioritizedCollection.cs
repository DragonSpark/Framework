using System.Collections.Generic;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark
{
	public class PrioritizedCollection<T> : CollectionBase<T>
	{
		public PrioritizedCollection() {}
		public PrioritizedCollection( IEnumerable<T> items ) : base( items ) {}
		public PrioritizedCollection( ICollection<T> source ) : base( source ) {}

		protected override IEnumerable<T> Query => base.Query.Prioritize();
	}
}