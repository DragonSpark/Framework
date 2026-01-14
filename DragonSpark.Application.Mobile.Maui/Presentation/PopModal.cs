using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using Microsoft.Maui.Controls;
using Switch = DragonSpark.Model.Results.Switch;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class PopModal : IAllocated<bool>
{
    public static PopModal Default { get; } = new();

    PopModal() : this(CurrentNavigation.Default, Popped.Default) {}
    
    readonly IResult<INavigation> _navigation;
    readonly ISwitch              _switch;

    public PopModal(IResult<INavigation> navigation, ISwitch @switch)
    {
        _navigation  = navigation;
        _switch = @switch;
    }

    public Task Get(bool parameter)
    {
        _switch.Up();
        return _navigation.Get().PopModalAsync(parameter);
    }
}

// TODO:

public sealed class Popped : Switch
{
    public static Popped Default { get; } = new();

    Popped() {}
}