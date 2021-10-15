using DragonSpark.Compose;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DragonSpark.Application.Security.Identity.Model;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RedirectErrorsAttribute : Attribute, IPageFilter
{
	public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
	{
		if (context.HandlerInstance is PageModel page &&
		    context.HandlerArguments.Only().Value is ErrorRedirect error)
		{
			page.TempData[error.Message.Key] = error.Message.Value;
			context.Result                   = page.RedirectToPage(error.Location, new {returnUrl = error.Origin});
		}
	}

	void IPageFilter.OnPageHandlerExecuted(PageHandlerExecutedContext context) {}

	void IPageFilter.OnPageHandlerSelected(PageHandlerSelectedContext context) {}
}