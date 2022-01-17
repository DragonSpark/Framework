using System;

namespace DragonSpark.Reflection.Assemblies;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class AssemblyBuildDateAttribute : Attribute
{
	public AssemblyBuildDateAttribute(string value) : this(DateTimeOffset.ParseExact(value, "o", null)) {}

	public AssemblyBuildDateAttribute(DateTimeOffset on) => On = on;

	public DateTimeOffset On { get; }
}