using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.MultiFactor;

public readonly record struct UserInput<T>(UserManager<T> Manager, T User) where T : class;