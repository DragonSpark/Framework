using System.Security.Claims;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity;

public interface IPrincipalAccess : IMutable<ClaimsPrincipal?>;