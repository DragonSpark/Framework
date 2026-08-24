using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Text;

namespace DragonSpark.Server.Output;

public class TagDefinition<TIn, TOut, T> : IStopAware<TagDefinitionInput<TIn, TOut>>
{
	readonly ICurrentTags            _tags;
	readonly ISelect<TOut, string>   _output;
	readonly IStopAware<TIn, string> _input;

	// ReSharper disable once TooManyDependencies
	protected TagDefinition(ICurrentTags tags, IFormatter<T> key, Func<TOut, T> select, IStopAware<TIn, T> input)
		: this(tags, Start.A.Selection(select).Then().Select(key).Get(), input.Then().Select(key).Out()) {}

	protected TagDefinition(ICurrentTags tags, ISelect<TOut, string> output, IStopAware<TIn, string> input)
	{
		_tags   = tags;
		_output = output;
		_input  = input;
	}

	public async ValueTask Get(Stop<TagDefinitionInput<TIn, TOut>> parameter)
	{
		var tags = _tags.Get();
		if (tags is not null)
		{
			var ((input, output), stop) = parameter;
			var item = output is not null ? _output.Get(output) : await _input.Off(new(input, stop));
			tags.Add(item);
		}
	}
}