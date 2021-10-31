using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Application.Security.Identity.Claims;

public sealed class IsCurrentAuthenticationMethod : Equaling<string>
{
	public IsCurrentAuthenticationMethod(CurrentAuthenticationMethod authentication)
		: base(authentication.Get, StringComparer.InvariantCultureIgnoreCase) {}
}