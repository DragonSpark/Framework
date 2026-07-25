using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class ComposeContextApplicationUser<T> : ISelecting<HttpContext, T> where T : class
{
    readonly IUsers<T> _users;

    public ComposeContextApplicationUser(IUsers<T> users) => _users = users;

    public async ValueTask<T> Get(HttpContext parameter)
    {
        using var session = _users.Get();
        var       result  = await session.Subject.GetUserAsync(parameter.User).Off();
        return result.Verify();
    }
}