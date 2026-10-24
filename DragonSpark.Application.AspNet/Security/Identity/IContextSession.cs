using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity;

public interface IContextSession<T> : ISelect<HttpContext, SignInManager<T>> where T : class;