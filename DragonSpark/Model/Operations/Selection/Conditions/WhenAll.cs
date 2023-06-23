using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Conditions;

public class WhenAll<T> : IDepending<T>
{
	readonly bool                  _continue;
	readonly uint                  _length;
	readonly ISelecting<T, bool>[] _all;

	protected WhenAll(params ISelecting<T, bool>[] all) : this(false, all) {}

	protected WhenAll(bool @continue, params ISelecting<T, bool>[] all)
		: this((uint)all.Length, @continue, all) {}

	protected WhenAll(uint length, bool @continue = false, params ISelecting<T, bool>[] all)
	{
		_continue = @continue;
		_length   = length;
		_all      = all;
	}

	public async ValueTask<bool> Get(T parameter)
	{
		for (var i = 0; i < _length; i++)
		{
			if (!await _all[i].Get(parameter).ConfigureAwait(_continue))
			{
				return false;
			}
		}

		return true;
	}
}