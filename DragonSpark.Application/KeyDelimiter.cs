using DragonSpark.Text;

namespace DragonSpark.Application
{
	public sealed class KeyDelimiter : Character
	{
		public static KeyDelimiter Default { get; } = new KeyDelimiter();

		KeyDelimiter() : base('+') {}
	}
}