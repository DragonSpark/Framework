using DragonSpark.Application.Navigation;
using DragonSpark.Presentation.Components.Navigation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;


namespace DragonSpark.Presentation.Components.Security
{
	public sealed class RedirectToAccessDenied : NavigateTo
	{
		public RedirectToAccessDenied() => Forced = true;

		[Parameter]
		public string FormatPath { get; set; } = AccessDeniedPathTemplate.Default;

		[Inject, UsedImplicitly]
		ILogger<RedirectToLogin> Logger { get; set; } = default!;

		[Inject]
		CurrentRootPath CurrentPath { get; set; } = default!;

		protected override void OnInitialized()
		{
			Path = new LoginPath(FormatPath).Get(CurrentPath.Get());

			Logger.LogDebug("Unauthorized resource '{Uri}' detected.  Redirecting to: {Redirect}",
			                Navigation.Uri, Path);
			base.OnInitialized();
		}
	}
}