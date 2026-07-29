using System.Diagnostics;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Results;

[DebuggerDisplay("{DebuggerToString(),nq}")]
public class Switch : Variable<bool>, ISwitch, ICondition
{
	public static implicit operator bool(Switch instance) => instance.Get();
	public static implicit operator Switch(bool instance) => new(instance);

	public Switch(bool instance = false) : base(instance) {}

	public bool Get(None parameter) => Get();

	string DebuggerToString() => Get().ToString();
}