using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

public interface IUserSynchronizer<T> : IDepending<Login<T>> where T : IdentityUser;