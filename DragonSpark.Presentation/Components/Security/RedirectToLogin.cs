using DragonSpark.Application.AspNet.Navigation;
using DragonSpark.Application.AspNet.Navigation.Security;
using DragonSpark.Application.Navigation;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Presentation.Components.Security;

/// <summary>
/// ATTRIBUTION: https://blog.vfrz.fr/blazor-redirect-non-authenticated-user/
/// </summary>
public sealed class RedirectToLogin : ComponentBase
{
	[Parameter]
	public required string FormatPath { get; set; }

	[Parameter]
	public bool Force { get; set; } = true;

	[Inject]
	ILogger<RedirectToLogin> Logger { get; set; } = null!;

	[Inject]
	CurrentRootPath CurrentPath { get; set; } = null!;
	
	[Inject]
	LoginPathTemplate LoginPathTemplate { get; set; } = null!;

	[Inject]
	NavigationManager Navigation { get; set; } = null!;

	protected override Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			var path = new TemplatedPath(FormatPath.Account() ?? LoginPathTemplate.Get()).Get(CurrentPath.Get());
			Logger.LogDebug("Unauthorized resource '{Uri}' detected.  Redirecting to: {Redirect}", Navigation.Uri, path);
			Navigation.NavigateTo(path, Force, true);
		}
		return base.OnAfterRenderAsync(firstRender);
	}
}