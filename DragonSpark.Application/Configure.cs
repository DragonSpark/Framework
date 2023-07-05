using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Alterations;
using System.Threading.Tasks;

namespace DragonSpark.Application;

sealed class Configure : IAlteration<BuildHostContext>
{
	public static Configure Default { get; } = new Configure();

	Configure() {}

	public BuildHostContext Get(BuildHostContext parameter)
		=> parameter.Configure(DefaultRegistrations.Default)
		            .Configure(Components.Registrations.Default)
		            .ComposeUsing(Entities.Compose.Default);
}

public readonly record struct Input();

public class SomeOperation : IOperation<Input>
{
	public ValueTask Get(Input parameter) => default;
}