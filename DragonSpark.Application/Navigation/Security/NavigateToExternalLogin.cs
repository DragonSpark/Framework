using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.Navigation.Security;

public class NavigateToExternalLogin : Navigation<string>
{
	public NavigateToExternalLogin(NavigationManager navigation, IAlteration<string> login)
		: base(navigation, login.Get, true) {}
}