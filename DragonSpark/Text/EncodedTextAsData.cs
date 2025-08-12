using System.Text;
using DragonSpark.Model.Selection;

namespace DragonSpark.Text;

public sealed class EncodedTextAsData : Select<string, byte[]>
{
	public static EncodedTextAsData Default { get; } = new();

	EncodedTextAsData() : this(Encoding.UTF8) {}

	public EncodedTextAsData(Encoding encoding) : base(encoding.GetBytes) {}
}