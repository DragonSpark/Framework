using System.Threading.Tasks;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class PopModal : IAllocated<bool>
{
    public static PopModal Default { get; } = new();

    PopModal() : this(CurrentNavigation.Default) {}
    
    readonly IResult<INavigation> _navigation;

    public PopModal(IResult<INavigation> navigation) => _navigation = navigation;

    public Task Get(bool parameter) => _navigation.Get().PopModalAsync(parameter);
}