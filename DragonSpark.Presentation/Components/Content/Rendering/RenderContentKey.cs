using DragonSpark.Application;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering
{
	sealed class RenderContentKey : IRenderContentKey
	{
		readonly ICurrentPrincipal _current;

		public RenderContentKey(ICurrentPrincipal current) => _current = current;

		public string Get(Delegate parameter)
			=> $"{parameter.Target.Verify().GetType().AssemblyQualifiedName}+{parameter.Method.ReturnType.AssemblyQualifiedName}+{_current.Get().UserName()}";
	}
}