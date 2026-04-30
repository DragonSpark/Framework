using System;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public readonly record struct ComposeUserDataInput<T>(UsersSession<T> Users, T? User, HttpContext Context)
    : IDisposable where T : class
{
    public void Dispose()
    {
        Users.Dispose();
    }
}