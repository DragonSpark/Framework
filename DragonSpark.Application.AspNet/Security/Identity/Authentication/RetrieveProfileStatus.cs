using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class RetrieveProfileStatus : Result<ProfileStatus>, IProfileStatus
{
    public RetrieveProfileStatus(ProfileStatusStore store) : base(store) {}
}