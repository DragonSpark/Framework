using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Security.Identity
{
	class Class1 {}

	// TODO: Move to runtime.
	public sealed class IdentifyingText : IResult<string>
	{
		public static IdentifyingText Default { get; } = new IdentifyingText();

		IdentifyingText() {}

		public string Get() => new Guid().ToString();
	}
}