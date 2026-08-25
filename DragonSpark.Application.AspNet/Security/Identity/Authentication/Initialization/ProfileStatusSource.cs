using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class ProfileStatusSource : CascadingValueSource<ProfileStatus>
{
	public ProfileStatusSource() : base(ProfileStatus.Anonymous, false) {}
}