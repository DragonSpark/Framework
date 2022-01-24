using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Assemblies;

sealed class AssemblyCreatedTime : ISelect<Assembly, DateTimeOffset>
{
	public static AssemblyCreatedTime Default { get; } = new();

	AssemblyCreatedTime() : this(Time.Default) {}

	readonly ITime _time;

	public AssemblyCreatedTime(ITime time) => _time = time;

	public DateTimeOffset Get(Assembly parameter)
		=> parameter.Attribute<AssemblyBuildDateAttribute>()?.On ?? _time.Get();
}