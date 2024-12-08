using DragonSpark.Compose;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RedirectErrorsAttribute : Attribute, IPageFilter
{
	public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
	{
		if (context.HandlerInstance is PageModel page &&
		    context.HandlerArguments.Only().Value is ErrorRedirect(var location, var (key, value), var origin))
		{
			page.TempData[key] = value;
			context.Result     = page.RedirectToPage(location, new { ReturnUrl = origin });
		}
	}

	void IPageFilter.OnPageHandlerExecuted(PageHandlerExecutedContext context) {}

	void IPageFilter.OnPageHandlerSelected(PageHandlerSelectedContext context) {}
}