using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class PopModal : IAllocated<bool>
{
	public static PopModal Default { get; } = new();

	PopModal() : this(CurrentNavigation.Default, Popped.Default) {}

	readonly IResult<INavigation>              _navigation;
	readonly DragonSpark.Model.Results.ISwitch _switch;

	public PopModal(IResult<INavigation> navigation, DragonSpark.Model.Results.ISwitch @switch)
	{
		_navigation = navigation;
		_switch     = @switch;
	}

	public Task Get(bool parameter)
	{
		_switch.Up();
		return _navigation.Get().PopModalAsync(parameter);
	}
}