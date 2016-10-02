using DragonSpark.Extensions;
using DragonSpark.Sources;
using System;

namespace DragonSpark.Runtime
{
	public interface IDisposables : IRepository<IDisposable>, IDisposable {}

	public sealed class Disposables : Scope<IDisposables>, IComposable<IDisposable>
	{
		public static Disposables Default { get; } = new Disposables();
		Disposables() : base( Factory.GlobalCache( () => new Repository() ) ) {}

		sealed class Repository : RepositoryBase<IDisposable>, IDisposables
		{
			readonly IDisposable disposable;

			public Repository() : base( new PurgingCollection<IDisposable>() )
			{
				disposable = new DelegatedDisposable( OnDispose );
			}

			void OnDispose() => this.Each( entry => entry.Dispose() );
			public void Dispose() => disposable.Dispose();
		}

		public void Add( IDisposable instance ) => Default.Get().Add( instance );
	}
}