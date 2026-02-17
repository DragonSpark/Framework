using System;

namespace DragonSpark.Text;

public sealed class TextAsData : Parser<byte[]>
{
	public static TextAsData Default { get; } = new();

	TextAsData() : base(Convert.FromBase64String) {}
}