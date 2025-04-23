using System.Security.Claims;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity.Profile;

public interface ICreateProfile : ISelect<ClaimsPrincipal, Profile>;