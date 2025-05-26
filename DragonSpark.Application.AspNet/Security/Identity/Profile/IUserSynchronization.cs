using DragonSpark.Model.Operations.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

public interface IUserSynchronization : IStopAware<ExternalLoginInfo>;