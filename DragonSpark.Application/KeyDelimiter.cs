using DragonSpark.Model.Results;

namespace DragonSpark.Application
{
	public sealed class KeyDelimiter : Instance<char>
	{
		public static KeyDelimiter Default { get; } = new KeyDelimiter();

		KeyDelimiter() : base('+') {}
	}
}
