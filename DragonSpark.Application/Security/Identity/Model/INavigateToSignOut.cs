using DragonSpark.Model.Commands;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model;

public interface INavigateToSignOut : ICommand<ClaimsPrincipal> {}