using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// ATTRIBUTION:
	/// https://github.com/ShaunCurtis/CEC.Routing/blob/master/CEC.Routing/Components/IRecordRoutingComponent.cs Defines
	/// the properties used during Routing to check the current page state The current page/component is registered in the
	/// User Session Service as a IRecordRoutingComponent object
	/// </summary>
	public interface IRoutingComponent
	{
		/// <summary>
		/// Injected Navigation Manager
		/// </summary>
		[Inject]
		public NavigationManager Navigation { get; set; }

		/// <summary>
		/// Injected User Session Object
		/// </summary>
		[Inject]
		public RouterSession Session { get; set; }

		/// <summary>
		/// Property to hold the current page Url
		/// We need this as the name of the component probably won't match the route
		/// </summary>
		public string? PageUrl { get; set; }

		public bool HasChanges { get; }
	}
}