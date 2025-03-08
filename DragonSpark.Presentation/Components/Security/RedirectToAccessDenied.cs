using DragonSpark.Application.AspNet.Navigation;
using DragonSpark.Application.AspNet.Navigation.Security;
using DragonSpark.Application.Navigation;
using DragonSpark.Presentation.Components.Navigation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;


namespace DragonSpark.Presentation.Components.Security;

public sealed class RedirectToAccessDenied : NavigateTo
{
	public RedirectToAccessDenied() => Forced = true;

	[Parameter]
	public string FormatPath { get; set; } = AccessDeniedPathTemplate.Default;

	[Inject, UsedImplicitly]
	ILogger<RedirectToAccessDenied> Logger { get; set; } = null!;

	[Inject]
	CurrentRootPath CurrentPath { get; set; } = null!;

	protected override void OnInitialized()
	{
		Path = new TemplatedPath(FormatPath).Get(CurrentPath.Get());

		Logger.LogDebug("Unauthorized resource '{Uri}' detected.  Redirecting to: {Redirect}",
		                Navigation.Uri, Path);
		base.OnInitialized();
	}
}