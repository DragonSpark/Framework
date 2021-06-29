using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Runtime
{
	public sealed class IdentifyingText : IResult<string>
	{
		public static IdentifyingText Default { get; } = new IdentifyingText();

		IdentifyingText() {}

		public string Get() => Guid.NewGuid().ToString();
	}
}