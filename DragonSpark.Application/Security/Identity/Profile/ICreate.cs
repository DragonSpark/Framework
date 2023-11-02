using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Profile;

public interface ICreate<T> : ISelecting<Login<T>, IdentityResult>;