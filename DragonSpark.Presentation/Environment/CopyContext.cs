using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class CopyContext : Alteration<HttpContext>, ICopyContext
{
	public static CopyContext Default { get; } = new();

	CopyContext() : base(CloneHttpContext.Default) {}
}