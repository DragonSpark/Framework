using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Application.Model.Text;

public sealed class EntryPath : IAlteration<string>
{
	public static EntryPath Default { get; } = new();

	EntryPath() : this(NewLeasing<char>.Default) {}

	readonly INewLeasing<char> _leasing;

	public EntryPath(INewLeasing<char> leasing) => _leasing = leasing;

	public string Get(string parameter)
	{
		var       input   = parameter.AsSpan();
		var       length  = input.Length;
		using var leasing = _leasing.Get((uint)length);
		var       into    = leasing.AsSpan();
		input.Replace(into, '\\', '/');
		var result = into.ToString();
		return result;
	}
}