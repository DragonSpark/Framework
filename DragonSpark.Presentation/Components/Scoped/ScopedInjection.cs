using System;

namespace DragonSpark.Presentation.Components.Scoped
{
	public readonly record struct ScopedInjection(Microsoft.AspNetCore.Components.ComponentBase Target,
	                                              IServiceProvider Provider);
}