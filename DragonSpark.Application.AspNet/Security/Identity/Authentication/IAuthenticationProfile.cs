using DragonSpark.Model.Operations.Results;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

interface IAuthenticationProfile : IResulting<ExternalLoginInfo?>;