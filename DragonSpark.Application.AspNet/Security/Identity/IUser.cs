using System.Security.Claims;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Security.Identity;

public interface IUser<out T> : ISelect<ClaimsPrincipal, T>;