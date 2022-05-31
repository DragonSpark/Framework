using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

sealed class RefreshActiveContent<T> : IUpdateMonitor
{
	readonly IResulting<T>  _result;
	readonly IMutable<bool> _state;
	readonly IMutable<int>  _counts;

	public RefreshActiveContent(IResulting<T> result, IMutable<int> counts)
		: this(result, new Variable<bool>(), counts) {}

	public RefreshActiveContent(IResulting<T> result, IMutable<bool> state, IMutable<int> counts)
	{
		_result = result;
		_state  = state;
		_counts = counts;
	}

	public async ValueTask Get(Action parameter)
	{
		_counts.Execute(0);
		var result = _result.Get();
		await result;
		_state.Execute(true);
		parameter();
	}

	public bool Get() => _state.Get();

	public void Execute(bool parameter)
	{
		_state.Execute(parameter);
	}
}