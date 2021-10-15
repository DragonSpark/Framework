using DragonSpark.Model.Results;
using System.Reflection;

namespace DragonSpark.Reflection;

public sealed class AllInstanceFlags : Instance<BindingFlags>
{
	public static AllInstanceFlags Default { get; } = new AllInstanceFlags();

	AllInstanceFlags() : base(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty) {}
}