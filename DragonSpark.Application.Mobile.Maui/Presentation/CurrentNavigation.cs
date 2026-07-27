using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class CurrentNavigation : IResult<INavigation>
{
    public static CurrentNavigation Default { get; } = new();

    CurrentNavigation() : this(CurrentPage.Default) {}
    
    readonly IResult<Page?> _page;

    public CurrentNavigation(IResult<Page?> page) => _page = page;

    public INavigation Get() => _page.Get().Verify().Navigation;
}