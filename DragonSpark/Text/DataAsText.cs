using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Text;

public sealed class DataAsText : Select<byte[], string>, IFormatter<byte[]>
{
	public static DataAsText Default { get; } = new();

	DataAsText() : base(Convert.ToBase64String) {}
}