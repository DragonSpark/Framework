using DragonSpark.Model.Operations.Selection.Stop.Conditions;

namespace DragonSpark.Application.AspNet.Security.Identity;

public interface IHasValidState<T> : IDepending<T> where T : IdentityUser;