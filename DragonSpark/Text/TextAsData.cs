using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Text;

public sealed class TextAsData : Select<string, byte[]>
{
	public static TextAsData Default { get; } = new();

	TextAsData() : base(Convert.FromBase64String) {}
}