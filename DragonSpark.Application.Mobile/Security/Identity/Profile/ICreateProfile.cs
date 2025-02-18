using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Mobile.Security.Identity.Profile;

public interface ICreateProfile : ISelect<ClaimsPrincipal, Profile>;