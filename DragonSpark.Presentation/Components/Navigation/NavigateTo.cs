using DragonSpark.Application.AspNet;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Navigation;

public class NavigateTo : ComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = null!;

    [Parameter]
    public string Path { get; set; } = null!;

    [Parameter]
    public bool Forced { get; set; }

    [Parameter]
    public bool Replace { get; set; }

    protected override void OnInitialized()
    {
        Navigate();
    }

    protected void Navigate()
    {
        var path = Path.Verify("Path not provided for navigation.");
        Navigation.NavigateTo(path, !Navigation.IsOn(path) && Forced, Replace);
    }
}