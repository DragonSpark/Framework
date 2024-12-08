using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Security.Identity;

public interface IHasValidState<in T> : IDepending<T> where T : IdentityUser;