using System;

namespace DragonSpark.Diagnostics
{
	public sealed class EmptyPolicy : Policy
	{
		public static EmptyPolicy Default { get; } = new EmptyPolicy();

		EmptyPolicy() : base(Polly.Policy.Handle<Exception>()) {}
	}
}