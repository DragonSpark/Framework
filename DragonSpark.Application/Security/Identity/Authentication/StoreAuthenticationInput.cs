using DragonSpark.Model.Sequences;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

readonly record struct StoreAuthenticationInput<T>(T User, Array<Claim> Claims);