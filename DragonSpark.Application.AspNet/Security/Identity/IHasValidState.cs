using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity;

public interface IHasValidState<T> : IDependingWithStop<T> where T : IdentityUser;