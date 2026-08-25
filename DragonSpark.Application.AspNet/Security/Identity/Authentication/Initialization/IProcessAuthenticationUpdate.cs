using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

public interface IProcessAuthenticationUpdate : IOperation<Task<AuthenticationState>>;