using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using System.Text;

namespace DragonSpark.Text;

public sealed class DecodedText : Alteration<string>
{
	public static DecodedText Default { get; } = new();

	DecodedText() : this(Encoding.UTF8) {}

	public DecodedText(Encoding encoding) : base(TextAsData.Default.Then().Subject.Select(encoding.GetString)) {}
}