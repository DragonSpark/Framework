using DragonSpark.Application.Navigation;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Security.Identity
{
	public class NavigateToExternalLogin : Navigation<string>
	{
		public NavigateToExternalLogin(NavigationManager navigation, IAlteration<string> login)
			: base(navigation, login.Get, true) {}
	}
}