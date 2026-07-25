using DragonSpark.Model.Sequences;
using DragonSpark.Text;

namespace DragonSpark.Azure.Ai;

sealed class Style : IText
{
	public static Style Default { get; } = new();

	Style() : this(Styles.Default, Random.Shared) {}

	readonly Array<string> _styles;
	readonly uint          _length;
	readonly Random        _random;

	public Style(Array<string> styles, Random random) : this(styles, styles.Length, random) {}

	public Style(Array<string> styles, uint length, Random random)
	{
		_styles = styles;
		_length = length;
		_random = random;
	}

	public string Get() => _styles[_random.Next((int)_length)];
}