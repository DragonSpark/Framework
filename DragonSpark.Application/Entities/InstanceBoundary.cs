using AsyncUtilities;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities;

public sealed class InstanceBoundary : IBoundary
{
	readonly AsyncLock _lock;
	readonly IToken    _token;

	public InstanceBoundary(DbContext context, IToken token) : this(Locks.Default.Get(context), token) {}

	public InstanceBoundary(AsyncLock @lock, IToken token)
	{
		_lock  = @lock;
		_token = token;
	}

	public async ValueTask<IDisposable> Get() => new Instance(await _lock.LockAsync(_token.Get()));

	sealed class Instance : Disposable<AsyncLock.Releaser>
	{
		public Instance(AsyncLock.Releaser disposable) : base(disposable) {}
	}
}