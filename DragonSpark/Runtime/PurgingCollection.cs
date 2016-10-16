using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Runtime
{
	public sealed class PurgingCollection<T> : CollectionBase<T>
	{
		readonly static IEnumerable<T> Items = Items<T>.Default.AsEnumerable();

		public PurgingCollection() : this( Items ) {}
		public PurgingCollection( IEnumerable<T> collection ) : base( collection ) {}
		public PurgingCollection( ICollection<T> source ) : base( source ) {}

		protected override IEnumerable<T> Query => Source.Purge().ToArray();
	}
}
