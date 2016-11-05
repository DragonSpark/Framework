using DragonSpark.Extensions;
using DragonSpark.Sources.Scopes;
using System;

namespace DragonSpark.Runtime
{
	public interface IDisposables : IRepository<IDisposable>, IDisposable {}

	public sealed class Disposables : SingletonScope<IDisposables>, IComposable<IDisposable>
	{
		public static Disposables Default { get; } = new Disposables();
		Disposables() : base( () => new Repository() ) {}

		public void Add( IDisposable instance ) => Get().Add( instance );

		sealed class Repository : RepositoryBase<IDisposable>, IDisposables
		{
			public Repository() : base( new PurgingCollection<IDisposable>() ) {}

			public void Dispose() => this.Each( entry => entry.Dispose() );
		}
	}
}