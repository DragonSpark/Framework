using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class CurrentContextUser<T> : ReferenceStoring<HttpContext, T> where T : class
{
    public CurrentContextUser(ComposeContextApplicationUser<T> previous) : base(previous) {}
}