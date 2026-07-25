using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Text;

public sealed class EncodedText : Alteration<string>
{
	public static EncodedText Default { get; } = new();

	EncodedText() : base(EncodedTextAsData.Default.Then().Subject.Select(Convert.ToBase64String)) {}
}