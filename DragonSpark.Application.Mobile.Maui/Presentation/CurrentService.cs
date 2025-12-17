using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class CurrentService<T> : Result<T> where T : notnull
{
    public static CurrentService<T> Default { get; } = new();

    CurrentService() : base(CurrentServices.Default.GetRequiredService<T>) {}
}

// TODO

public sealed class CurrentPage : IResult<Page?>
{
    public static CurrentPage Default { get; } = new();

    CurrentPage() : this(Microsoft.Maui.Controls.Application.Current.Verify().Windows.Select(x => x.Page)) {}

    readonly IEnumerable<Page?> _windows;

    public CurrentPage(IEnumerable<Page?> windows) => _windows = windows;

    public Page? Get() => _windows.First(x => x is not null);
}
// TODO
public sealed class CurrentNavigation : IResult<INavigation>
{
    public static CurrentNavigation Default { get; } = new();

    CurrentNavigation() : this(CurrentPage.Default) {}
    
    readonly IResult<Page?> _page;

    public CurrentNavigation(IResult<Page?> page) => _page = page;

    public INavigation Get() => _page.Get().Verify().Navigation;
}

public readonly record struct PushModalInput(Page Subject, bool Animated = true);
public sealed class PushModal : IAllocated<PushModalInput>
{
    public static PushModal Default { get; } = new();

    PushModal() : this(CurrentNavigation.Default) {}
    
    readonly IResult<INavigation> _navigation;

    public PushModal(IResult<INavigation> navigation) => _navigation = navigation;

    public Task Get(PushModalInput parameter)
    {
        var (subject, animated) = parameter;
        return _navigation.Get().PushModalAsync(subject, animated);
    }
}
public sealed class PopModal : IAllocated<bool>
{
    public static PopModal Default { get; } = new();

    PopModal() : this(CurrentNavigation.Default) {}
    
    readonly IResult<INavigation> _navigation;

    public PopModal(IResult<INavigation> navigation) => _navigation = navigation;

    public Task Get(bool parameter) => _navigation.Get().PopModalAsync(parameter);
}