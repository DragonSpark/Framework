using Polly;
using System;

namespace DragonSpark.Application.Entities.Diagnostics
{
	public sealed class ApplicationContentPolicy : DragonSpark.Diagnostics.Policy
	{
		public static ApplicationContentPolicy Default { get; } = new ApplicationContentPolicy();

		ApplicationContentPolicy()
			: base(Policy.Handle<InvalidOperationException>(IsApplicationContentException.Default.Get)) {}
	}
}