using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

public sealed class IsDeployed : DelegatedResultCondition
{
	[UsedImplicitly]
	public static IsDeployed Default { get; } = new();

	IsDeployed() : this(IsAssemblyDeployed.Default, PrimaryAssembly.Default) {}

	public IsDeployed(ICondition<Assembly> condition, IResult<Assembly> result)
		: base(condition.Then().Bind(result).Singleton()) {}
}