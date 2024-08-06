using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing;

public abstract class NavigationAwareComponent : ComponentBase, IAsyncDisposable
{
	IDisposable _registration = null!;

	protected override void OnInitialized()
	{
		_registration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
		base.OnInitialized();
	}

	async ValueTask OnLocationChanging(LocationChangingContext parameter)
	{
		if (await Allow(parameter))
		{
			await Exit().Await();
		}
		else
		{
			parameter.PreventNavigation();
			await OnNavigationCanceled().Await();
		}
	}

	protected abstract ValueTask<bool> Allow(LocationChangingContext parameter);

	[Inject]
	protected NavigationManager Navigation { get; set; } = default!;

	[Parameter]
	public EventCallback Exited { get; set; }

	protected virtual Task OnNavigationCanceled() => Task.CompletedTask;

	protected virtual Task Exit() => Exited.Invoke();

	protected virtual void OnDispose(bool disposing) {}

	protected virtual ValueTask OnDisposing()
	{
		OnDispose(true);
		_registration.Dispose();
		return ValueTask.CompletedTask;
	}

	public async ValueTask DisposeAsync()
	{
		await OnDisposing().Await();
		GC.SuppressFinalize(this);
	}

	~NavigationAwareComponent()
	{
		OnDispose(false);
	}
}