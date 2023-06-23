using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

public interface IRemoveLogin<T> : ISelecting<RemoveLoginInput<T>, IdentityResult> {}