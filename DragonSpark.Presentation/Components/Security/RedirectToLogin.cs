using DragonSpark.Application.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Security;

/// <summary>
/// ATTRIBUTION: https://blog.vfrz.fr/blazor-redirect-non-authenticated-user/
/// </summary>
public sealed class RedirectToLogin : ComponentBase
{
	/*public RedirectToLogin() => Forced = true;*/

	[Parameter]
	public string FormatPath { get; set; } = LoginPathTemplate.Default;

	[Inject]
	ILogger<RedirectToLogin> Logger { get; set; } = default!;

	[Inject]
	CurrentRootPath CurrentPath { get; set; } = default!;

	[Inject]
	NavigationManager Navigation { get; set; } = default!;

	[Inject]
	IJSRuntime Runtime { get; set; } = default!;

	protected override Task OnAfterRenderAsync(bool firstRender)
	{
		var path = new TemplatedPath(FormatPath).Get(CurrentPath.Get());

		Logger.LogDebug("Unauthorized resource '{Uri}' detected.  Redirecting to: {Redirect}", Navigation.Uri, path);
		return firstRender ? Runtime.InvokeVoidAsync("location.replace", path).AsTask() : base.OnAfterRenderAsync(firstRender);
	}
}