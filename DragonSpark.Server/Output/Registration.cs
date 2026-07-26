using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Output;

sealed class Registration<TIn, TKey> : IRegistration
{
	readonly Func<TIn, TKey> _select;

	public Registration(Func<TIn, TKey> select) => _select = select;

	public ValueTask Get(Stop<ComposeTagsInput> parameter)
	{
		var ((input, key, result), _) = parameter;
		if (input is TIn @in && key is IOutputKey<TKey> k)
		{
			result.Add(k.Get(_select(@in)));
		}

		return ValueTask.CompletedTask;
	}
}