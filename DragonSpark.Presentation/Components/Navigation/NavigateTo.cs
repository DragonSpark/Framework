using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Navigation;

public class NavigateTo : ComponentBase
{
	[Inject]
	protected NavigationManager Navigation { get; set; } = default!;

	[Parameter]
	public string Path { get; set; } = default!;

	[Parameter]
	public bool Forced { get; set; }

	protected override void OnInitialized()
	{
		Navigate();
	}

	protected void Navigate()
	{
		var path = Path.Verify("Path not provided for navigation.");
		if (path.TrimStart('/') != Navigation.ToBaseRelativePath(Navigation.Uri))
		{
			Navigation.NavigateTo(path, Forced);
		}
	}
}