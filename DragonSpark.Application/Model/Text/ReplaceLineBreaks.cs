namespace DragonSpark.Application.Model.Text;

sealed class ReplaceLineBreaks : Replace
{
	public static ReplaceLineBreaks Default { get; } = new();

	ReplaceLineBreaks() : base(new(@"([^\s\\])\r?\n"), "$1<br />") {}
}