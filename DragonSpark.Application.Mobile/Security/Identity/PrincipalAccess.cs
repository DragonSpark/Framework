using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.Mobile.Security.Identity;

sealed class PrincipalAccess : Variable<ClaimsPrincipal>, IPrincipalAccess;