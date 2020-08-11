using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// Attribution: https://github.com/ShaunCurtis/CEC.Routing/blob/master/CEC.RoutingSample/Components/EditorComponentBase.cs
	/// </summary>
	public class EditorComponent : ComponentBase, IRoutingComponent, IDisposable
	{
		[Inject]
		public NavigationManager Navigation { get; set; } = default!;

		[Inject]
		public RouterSession? Session { get; set; } = default!;

		public string? PageUrl { get; set; }

		protected virtual bool HasChanges() => EditContext?.IsModified() ?? false;


		bool IRoutingComponent.HasChanges => HasChanges();

		/// <summary>
		/// Form Edit Context
		/// </summary>
		[CascadingParameter]
		EditContext EditContext { get; set; } = default!;

		/*/// <summary>
		/// Alert object used in UI by UI Alert
		/// </summary>
		public Alert Alert { get; set; } = new Alert();*/

		protected override Task OnInitializedAsync()
		{
			PageUrl                     =  Navigation.Uri;
			Session!.ActiveComponent    =  this;
			Session.NavigationCanceled += OnNavigationCanceled;
			return base.OnInitializedAsync();
		}

		/// <summary>
		/// Event Handler for the Navigation Cancelled event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnNavigationCanceled(object? sender, EventArgs e)
		{
			OnNavigationCanceled();
		}

		protected virtual void OnNavigationCanceled() => StateHasChanged();

		public void Continue()
		{
			var session = Session.Verify();
			session.ActiveComponent = null;
			Navigation.Verify().NavigateTo(session.NavigationCancelledUrl);
		}

		public void Dispose()
		{
			Session!.NavigationCanceled -= OnNavigationCanceled;
		}
	}

}
