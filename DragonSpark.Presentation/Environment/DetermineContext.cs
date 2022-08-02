using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class DetermineContext : Alteration<HttpContext>, IDetermineContext
{
	public static DetermineContext Default { get; } = new();

	DetermineContext() : base(CloneHttpContext.Default) {}
}