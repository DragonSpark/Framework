using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components
{
	public class Navigation : Command
	{
		public Navigation(NavigationManager navigation, string path, bool force = false)
			: base(new Navigate(navigation, force).Then().Bind(path)) {}
	}
}