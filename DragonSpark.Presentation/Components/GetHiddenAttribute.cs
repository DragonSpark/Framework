using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime;
using Humanizer;
using Microsoft.FluentUI.AspNetCore.Components;

namespace DragonSpark.Presentation.Components;

sealed class GetHiddenAttribute : ISelect<GridItemHidden?, string?>
{
	public static GetHiddenAttribute Default { get; } = new();

	GetHiddenAttribute()
		: this(GridItemHidden.Xs, GridItemHidden.Sm, GridItemHidden.Md, GridItemHidden.Lg, GridItemHidden.Xl,
		       GridItemHidden.Xxl) {}

	readonly Array<GridItemHidden> _values;

	public GetHiddenAttribute(params GridItemHidden[] values) : this(values.Result()) {}

	public GetHiddenAttribute(Array<GridItemHidden> values) => _values = values;

	public string? Get(GridItemHidden? parameter)
	{
		if (parameter is {} p)
		{
			using var builder = ArrayBuilder.New<string>(_values.Length);
			foreach (var value in _values)
			{
				if ((p & value) == value)
				{
					builder.UncheckedAdd(value.Humanize(LetterCasing.LowerCase));
				}
			}

			if (builder.AsSpan() is { Length: > 0 } span)
			{
				return string.Join(' ', span);
			}
			
		}

		return null;
	}
}