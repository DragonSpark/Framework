using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Authentication;

interface IAuthenticationProfile : IResulting<ExternalLoginInfo?> {}