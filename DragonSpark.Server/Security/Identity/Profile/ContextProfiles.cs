using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Security.Identity.Profile;

sealed class ContextProfiles : ReferenceStoring<HttpContext, ProfileBase>
{
    public ContextProfiles(ICreateProfile create) : base(x => create.Get(x.User.Stop(x.RequestAborted))) {}
}