using DragonSpark.Application.Navigation;
using DragonSpark.Application.Navigation.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Security;

/// <summary>
/// ATTRIBUTION: https://blog.vfrz.fr/blazor-redirect-non-authenticated-user/
/// </summary>
public sealed class RedirectToLogin : ComponentBase
{
	[Parameter]
	public string FormatPath { get; set; } = LoginPathTemplate.Default;

	[Parameter]
	public bool Force { get; set; } = true;

	[Inject]
	ILogger<RedirectToLogin> Logger { get; set; } = default!;

	[Inject]
	CurrentRootPath CurrentPath { get; set; } = default!;

	[Inject]
	NavigationManager Navigation { get; set; } = default!;

	protected override Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			var path = new TemplatedPath(FormatPath).Get(CurrentPath.Get());
			Logger.LogDebug("Unauthorized resource '{Uri}' detected.  Redirecting to: {Redirect}", Navigation.Uri, path);
			Navigation.NavigateTo(path, Force, true);
		}
		return base.OnAfterRenderAsync(firstRender);
	}
}