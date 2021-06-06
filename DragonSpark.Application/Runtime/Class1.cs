using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Runtime
{
	class Class1 {}

	public sealed class IdentifyingText : IResult<string>
	{
		public static IdentifyingText Default { get; } = new IdentifyingText();

		IdentifyingText() {}

		public string Get() => new Guid().ToString();
	}
}