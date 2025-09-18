using DragonSpark.Model;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;

namespace DragonSpark.Azure.Storage;

sealed class EntryTable : ITable<string, string?>
{
	readonly ITable<string, string?> _previous;
	readonly IAlteration<string>     _encode, _decode;

	public EntryTable(ITable<string, string?> previous)
		: this(previous, EncodedText.Default, DecodedText.Default) {}

	public EntryTable(ITable<string, string?> previous, IAlteration<string> encode, IAlteration<string> decode)
	{
		_previous = previous;
		_encode   = encode;
		_decode   = decode;
	}

	public ICondition<string> Condition => _previous.Condition;

	public string? Get(string parameter)
	{
		var previous = _previous.Get(parameter);
		return previous is not null
			       ? System.Buffers.Text.Base64.IsValid(previous) ? _decode.Get(previous) : previous
			       : null;
	}

	public void Execute(Pair<string, string?> parameter)
	{
		var (key, value) = parameter;
		_previous.Execute(new(key, value is not null ? _encode.Get(value) : null));
	}

	public bool Remove(string key) => _previous.Remove(key);
}