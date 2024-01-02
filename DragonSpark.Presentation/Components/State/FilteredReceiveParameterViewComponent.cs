using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Conditions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public abstract class FilteredReceiveParameterViewComponent<T> : ReceiveParameterViewComponent<T> where T : notnull
{
	readonly static IDepending<T> DefaultCondition = Is.Always<T>().Operation().Out();

	[Parameter]
	public IDepending<T> Condition { get; set; } = DefaultCondition;

	protected override async Task OnReceive(T parameter)
	{
		if (await Condition.Get(parameter))
		{
			await base.OnReceive(parameter).ConfigureAwait(false);
		}
	}
}