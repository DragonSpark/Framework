using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class AssignProfileStatus : Command<ProfileStatus>, IAssignProfileStatus
{
    public AssignProfileStatus(ProfileStatusStore command) : base(command) {} }