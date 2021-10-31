using DragonSpark.Model.Selection.Alterations;
using System;
using System.Text;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

sealed class FormatKey : IAlteration<string>
{
	public static FormatKey Default { get; } = new();

	FormatKey() : this(' ', 4) {}

	readonly char _space;
	readonly byte _spaces;

	public FormatKey(char space, byte spaces)
	{
		_space  = space;
		_spaces = spaces;
	}

	public string Get(string parameter)
	{
		var result = new StringBuilder();
		var index  = 0;
		var length = parameter.Length;
		while (index + _spaces < length)
		{
			result.Append(parameter.AsSpan(index, _spaces)).Append(_space);
			index += _spaces;
		}

		if (index < length)
		{
			result.Append(parameter.AsSpan(index));
		}

		return result.ToString().ToLowerInvariant();
	}
}