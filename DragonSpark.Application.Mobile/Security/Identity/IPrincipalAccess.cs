using DragonSpark.Model.Results;
using System.Security.Claims;

namespace DragonSpark.Application.Mobile.Security.Identity;

public interface IPrincipalAccess : IMutable<ClaimsPrincipal?>;