using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Presentation.Components
{
	/// <summary>
	/// ATTRIBUTION: https://blog.vfrz.fr/blazor-redirect-non-authenticated-user/
	/// </summary>
	public sealed class RedirectToLogin : ComponentBase
	{
		[Inject, UsedImplicitly]
		NavigationManager Navigation { get; set; }



		[Inject, UsedImplicitly]
		ILogger<RedirectToLogin> Logger { get; set; }


		[Parameter]
		public string LoginPath { get; set; } = "Identity/Account/Login?returnUrl=/{0}";

		protected override void OnInitialized()
		{
			var returnUrl = string.Format(LoginPath, Navigation.ToBaseRelativePath(Navigation.Uri));



			Logger.LogDebug("Unauthorized resource '{Uri}' detected.  Redirecting to: {Redirect}",
			                Navigation.Uri, returnUrl);
			Navigation.NavigateTo(returnUrl, true);
		}
	}
}
