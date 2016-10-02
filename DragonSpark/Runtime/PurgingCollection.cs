using DragonSpark.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Runtime
{
	public class PurgingCollection<T> : CollectionBase<T>
	{
		public PurgingCollection() {}
		public PurgingCollection( IEnumerable<T> collection ) : base( collection ) {}
		public PurgingCollection( ICollection<T> source ) : base( source ) {}

		protected override IEnumerable<T> Query => Source.Purge().ToArray();
	}
}
