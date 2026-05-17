using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Model.Sequences.Collections;

public sealed class Cast : ISelect<CastInput, System.Array>
{
	public static Cast Default { get; } = new();

	Cast() {}
	
	public System.Array Get(CastInput parameter)
	{
		var (input, to) = parameter;
		var result       = System.Array.CreateInstance(to, input.Length);

		for (var i = 0; i < input.Length; i++)
		{
			result.SetValue(Convert.ChangeType(input[i], to), i);
		}

		return result;
	}
}