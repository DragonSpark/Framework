using DragonSpark.Model.Operations.Results;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Authentication;

interface IAuthenticationProfile : IResulting<ExternalLoginInfo?> {}