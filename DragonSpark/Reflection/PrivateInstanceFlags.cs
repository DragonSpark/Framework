using DragonSpark.Model.Results;
using System.Reflection;

namespace DragonSpark.Reflection;

public sealed class PrivateInstanceFlags : Instance<BindingFlags>
{
	public static PrivateInstanceFlags Default { get; } = new PrivateInstanceFlags();

	PrivateInstanceFlags() : base(BindingFlags.NonPublic | BindingFlags.Instance) {}
}