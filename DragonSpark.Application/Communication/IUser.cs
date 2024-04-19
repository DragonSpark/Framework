using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Communication;

public interface IUser<out T> : ISelect<ClaimsPrincipal, T>;