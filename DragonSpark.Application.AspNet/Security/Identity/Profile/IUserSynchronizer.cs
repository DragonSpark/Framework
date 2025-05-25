using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

public interface IUserSynchronizer<T> : IDependingWithStop<Login<T>> where T : IdentityUser;