using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Text;

public sealed class TextAsData : Select<string, byte[]>, IParser<byte[]>
{
	public static TextAsData Default { get; } = new();

	TextAsData() : base(Convert.FromBase64String) {}
}