using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// Attribution:
	/// https://github.com/ShaunCurtis/CEC.Routing/blob/master/CEC.RoutingSample/Components/EditorComponentBase.cs
	/// </summary>
	public class EditorComponent : ComponentBase, IRoutingComponent, IDisposable
	{
		[Inject]
		public NavigationManager Navigation { get; set; } = default!;

		[Inject]
		public RouterSession Session { get; set; } = default!;

		[Parameter]
		public EventCallback Exited { get; set; }

		public string? PageUrl { get; set; }

		public virtual bool HasChanges => EditContext.IsModified();

		/// <summary>
		/// Form Edit Context
		/// </summary>
		[CascadingParameter]
		public EditContext EditContext { get; set; } = default!;

		/*/// <summary>
		/// Alert object used in UI by UI Alert
		/// </summary>
		public Alert Alert { get; set; } = new Alert();*/

		protected override Task OnInitializedAsync()
		{
			PageUrl                    =  Navigation.Uri;
			Session!.ActiveComponent   =  this;
			Session.NavigationCanceled += OnNavigationCanceled;
			return base.OnInitializedAsync();
		}

		void OnNavigationCanceled(object? sender, EventArgs e)
		{
			OnNavigationCanceled();
		}

		protected virtual void OnNavigationCanceled() => StateHasChanged();

		protected Task Exit()
		{
			EditContext.MarkAsUnmodified();

			var destination = Session.NavigationCancelledUrl;
			if (destination != null)
			{
				Navigation.NavigateTo(destination);
			}

			return Exited.InvokeAsync(this);
		}

		public void Dispose()
		{
			Session.NavigationCanceled -= OnNavigationCanceled;
		}
	}
}