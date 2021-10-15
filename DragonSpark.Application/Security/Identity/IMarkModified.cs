using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity;

public interface IMarkModified<in T> : IOperation<T> where T : IdentityUser {}