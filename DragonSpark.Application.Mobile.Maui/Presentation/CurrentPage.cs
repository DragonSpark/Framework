using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class CurrentPage : IResult<Page?>
{
    public static CurrentPage Default { get; } = new();

    CurrentPage() : this(Microsoft.Maui.Controls.Application.Current.Verify().Windows.Select(x => x.Page)) {}

    readonly IEnumerable<Page?> _windows;

    public CurrentPage(IEnumerable<Page?> windows) => _windows = windows;

    public Page? Get() => _windows.First(x => x is not null);
}