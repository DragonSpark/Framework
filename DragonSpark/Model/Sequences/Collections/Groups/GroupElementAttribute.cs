using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Model.Sequences.Collections.Groups;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GroupElementAttribute : Attribute, IResult<string>
{
	readonly string _name;

	public GroupElementAttribute(string name) => _name = name;

	public string Get() => _name;
}