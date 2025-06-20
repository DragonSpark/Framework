using System.Security.Claims;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Security.Identity.Profile;

public interface ICreateProfile : IStopAware<ClaimsPrincipal, ProfileBase>;