using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity;

public interface IUser<out T> : ISelect<ClaimsPrincipal, T>;