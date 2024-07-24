namespace DragonSpark.Application.Model.Text;

sealed class ReplaceTabs : Replace
{
	public static ReplaceTabs Default { get; } = new();

	ReplaceTabs() : base(new(@"\t"), "&nbsp;&nbsp;&nbsp;&nbsp;") {}
}