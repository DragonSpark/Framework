using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Profile;

public interface ICreateRequest : ISelecting<ExternalLoginInfo, CreateRequestResult> {}

// TODO

public readonly record struct CreateRequestResult(IdentityResult Result, IdentityUser? User);