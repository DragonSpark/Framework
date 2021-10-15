using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Model.Sequences.Collections.Groups;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InsertGroupElementAttribute : Attribute, IResult<int>
{
	readonly int _index;

	public InsertGroupElementAttribute() : this(0) {}

	public InsertGroupElementAttribute(int index) => _index = index;

	public int Get() => _index;
}