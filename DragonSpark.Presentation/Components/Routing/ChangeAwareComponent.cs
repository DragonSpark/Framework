using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// Attribution:
	/// https://github.com/ShaunCurtis/CEC.Routing/blob/master/CEC.RoutingSample/Components/EditorComponentBase.cs
	/// </summary>
	public abstract class ChangeAwareComponent : ComponentBase, IRoutingComponent, IDisposable
	{
		readonly Func<Task> _cancel;

		protected ChangeAwareComponent() => _cancel = OnNavigationCanceled;

		[Inject]
		public NavigationManager Navigation { get; set; } = default!;

		[Inject]
		public RouterSession Session { get; set; } = default!;

		[Parameter]
		public EventCallback Exited { get; set; }

		public string? PageUrl { get; set; }

		public abstract bool HasChanges { get; }

		protected override Task OnInitializedAsync()
		{
			PageUrl                    =  Navigation.Uri;
			Session!.ActiveComponent   =  this;
			Session.NavigationCanceled += OnNavigationCanceled;
			return base.OnInitializedAsync();
		}

		void OnNavigationCanceled(object? sender, EventArgs e)
		{
			InvokeAsync(_cancel);
		}

		protected virtual Task OnNavigationCanceled()
		{
			StateHasChanged();
			return Task.CompletedTask;
		}

		protected virtual Task Exit()
		{
			var destination = Session.NavigationCancelledUrl;
			if (destination != null)
			{
				Navigation.NavigateTo(destination);
			}

			return Exited.InvokeAsync(this);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Session.NavigationCanceled -= OnNavigationCanceled;
			Session.ActiveComponent    =  Session.ActiveComponent == this ? null : Session.ActiveComponent;
		}
	}
}