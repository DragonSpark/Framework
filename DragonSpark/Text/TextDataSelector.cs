using DragonSpark.Model.Selection;
using System.Text;

namespace DragonSpark.Text
{
	sealed class TextDataSelector : Select<string, byte[]>
	{
		public static TextDataSelector Default { get; } = new TextDataSelector();

		TextDataSelector() : this(Encoding.UTF8) {}

		public TextDataSelector(Encoding encoding) : base(encoding.GetBytes) {}
	}
}