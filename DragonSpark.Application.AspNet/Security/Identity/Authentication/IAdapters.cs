using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public interface IAdapters : IAlteration<Task<AuthenticationState>>;