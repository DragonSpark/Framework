using DragonSpark.Application.Navigation;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Navigation
{
	public sealed class NavigateToInitialize : DragonSpark.Application.Navigation.Navigation
	{
		public NavigateToInitialize(NavigationManager navigation, CurrentPath current)
			: base(navigation, InitializePath.Default.Then().Bind(current), true) {}
	}
}