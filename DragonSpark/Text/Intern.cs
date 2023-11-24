using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Text;

public sealed class Intern : Alteration<string>
{
	public static Intern Default { get; } = new();

	Intern() : base(string.Intern) {}
}