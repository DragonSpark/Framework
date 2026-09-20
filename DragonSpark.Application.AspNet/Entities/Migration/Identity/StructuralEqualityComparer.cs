using DragonSpark.Model.Selection.Conditions;
using System.Collections;
using System.Globalization;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class StructuralEqualityComparer : IEqualityComparer<object?>
{
	public readonly static StructuralEqualityComparer Default = new();

	StructuralEqualityComparer() : this(IsStructurallyEqual.Default, IsFloatingPoint.Default) {}

	readonly IEqualityComparer  _previous;
	readonly ICondition<object> _floating;

	public StructuralEqualityComparer(IEqualityComparer previous, ICondition<object> floating)
	{
		_previous = previous;
		_floating = floating;
	}

	bool IEqualityComparer<object?>.Equals(object? x, object? y)
		=> ReferenceEquals(x, y) || x?.GetHashCode() == y?.GetHashCode() || _previous.Equals(x, y);

	public int GetHashCode(object obj)
	{
		if (obj is IConvertible c)
		{
			try
			{
				return _floating.Get(c)
					       ? Convert.ToDouble(c, CultureInfo.InvariantCulture).GetHashCode()
					       : Convert.ToInt64(c, CultureInfo.InvariantCulture).GetHashCode();
			}
			catch
			{
				// Fallback on error
			}
		}

		return _previous.GetHashCode(obj);
	}
}