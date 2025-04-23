using System.Security.Claims;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity;

sealed class PrincipalAccess : Variable<ClaimsPrincipal>, IPrincipalAccess;