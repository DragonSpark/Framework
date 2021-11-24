using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Text;

public sealed class DataAsText : Select<byte[], string>
{
	public static DataAsText Default { get; } = new();

	DataAsText() : base(Convert.ToBase64String) {}
}