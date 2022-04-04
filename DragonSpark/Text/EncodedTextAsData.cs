using DragonSpark.Model.Selection;
using System.Text;

namespace DragonSpark.Text;

public sealed class EncodedTextAsData : Select<string, byte[]>
{
	public static EncodedTextAsData Default { get; } = new EncodedTextAsData();

	EncodedTextAsData() : this(Encoding.UTF8) {}

	public EncodedTextAsData(Encoding encoding) : base(encoding.GetBytes) {}
}