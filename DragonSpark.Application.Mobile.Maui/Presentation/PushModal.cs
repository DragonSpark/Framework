using System.Threading.Tasks;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

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