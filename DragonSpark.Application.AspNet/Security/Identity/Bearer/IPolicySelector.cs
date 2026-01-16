using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public interface IPolicySelector : ISelect<HttpContext, string?>;