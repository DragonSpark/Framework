using DragonSpark.Compose;
using DragonSpark.Composition.Scopes.Hierarchy;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Scoped;

public abstract class OwningComponentBase : ComponentBase, IDisposable, IAsyncDisposable
{
	IScopedServiceProvider? _services;

	[Inject]
	IScopedServices Services { get; set; } = default!;

	protected bool IsDisposed { get; private set; }

	protected IServiceProvider ScopedServices
	{
		get
		{
			if (Services == null)
			{
				throw new InvalidOperationException("Services cannot be accessed before the component is initialized.");
			}

			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}

			_services ??= Services.Get();
			return _services;
		}
	}

	void IDisposable.Dispose()
	{
		if (!IsDisposed)
		{
			_services?.Dispose();
			_services = null;
			Dispose(true);
			IsDisposed = true;
		}
	}

	protected virtual void Dispose(bool disposing) {}

	public virtual ValueTask DisposeAsync()
	{
		Dispose(true);
		return _services?.DisposeAsync() ?? ValueTask.CompletedTask;
	}
}


public class OwningComponentBase<T> : OwningComponentBase where T : class
{
	T? _item;

	protected T Service
	{
		get
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}

			// We cache this because we don't know the lifetime. We have to assume that it could be transient.
			// ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
			_item ??= ScopedServices.GetRequiredService<T>();
			return _item;
		}
	}

	protected override Task OnInitializedAsync() => Execute.Get(GetType(), Initialize()).AsTask();

	public override async ValueTask DisposeAsync()
	{
		await base.DisposeAsync();
		if (_item is IAsyncDisposable disposable)
		{
			await disposable.DisposeAsync();
		}
	}
}