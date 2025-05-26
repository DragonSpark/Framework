using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

public interface IRemoveLogin<T> : IStopAware<RemoveLoginInput<T>, IdentityResult>;