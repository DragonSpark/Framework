using System.Security.Claims;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Identity.Profile;

public interface ICreateProfile : ISelect<ClaimsPrincipal, ProfileBase>;