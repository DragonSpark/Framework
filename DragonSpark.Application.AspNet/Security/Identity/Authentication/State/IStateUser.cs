using DragonSpark.Model.Operations.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.State;

public interface IStateUser<T> : ISelecting<ClaimsPrincipal, T?> where T : IdentityUser;