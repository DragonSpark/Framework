using DragonSpark.Sources;
using System.Collections.Generic;

namespace DragonSpark.Runtime
{
	public abstract class RepositoryBase<T> : ItemSourceBase<T>, IRepository<T>
	{
		protected RepositoryBase() : this( new List<T>() ) {}

		protected RepositoryBase( IEnumerable<T> items ) : this( new List<T>( items ) ) {}

		protected RepositoryBase( ICollection<T> source )
		{
			Source = source;
		}

		protected ICollection<T> Source { get; }

		public void Add( T instance ) => OnAdd( instance );

		protected virtual void OnAdd( T entry ) => Source.Add( entry );

		protected override IEnumerable<T> Yield() => Source;
	}
}