using DragonSpark.Text;
using System;

namespace DragonSpark.Model
{
	public sealed class ExpectedTypeMessage : IMessage<Type>
	{
		readonly Type _expected;

		public ExpectedTypeMessage(Type expected) => _expected = expected;

		public string Get(Type parameter) => $"'{parameter}' is not of expected type '{_expected}'.";
	}
}