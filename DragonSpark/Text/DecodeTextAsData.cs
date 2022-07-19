using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using System.Text;

namespace DragonSpark.Text;

public sealed class DecodeTextAsData : Alteration<string>
{
	public static DecodeTextAsData Default { get; } = new();

	DecodeTextAsData() : this(Encoding.UTF8) {}

	public DecodeTextAsData(Encoding encoding) : base(TextAsData.Default.Then().Subject.Select(encoding.GetString)) {}
}