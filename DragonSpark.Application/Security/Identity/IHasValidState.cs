using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity;

public interface IHasValidState<in T> : IDepending<T> where T : IdentityUser {}