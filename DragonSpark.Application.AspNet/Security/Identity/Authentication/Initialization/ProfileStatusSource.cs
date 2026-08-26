using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class ProfileStatusSource : CascadingValueSource<ProfileStatus?>
{
	public ProfileStatusSource(ProfileStatusValue value) : base(value.Get, false) {}
}