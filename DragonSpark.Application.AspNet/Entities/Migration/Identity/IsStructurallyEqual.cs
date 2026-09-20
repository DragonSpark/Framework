using DragonSpark.Model.Selection.Conditions;
using System.Collections;
using System.Globalization;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class IsStructurallyEqual : IEqualityComparer
{
	public static IsStructurallyEqual Default { get; } = new();

	IsStructurallyEqual() : this(StructuralComparisons.StructuralEqualityComparer, IsFloatingPoint.Default) {}

	readonly IEqualityComparer  _previous;
	readonly ICondition<object> _floating;

	public IsStructurallyEqual(IEqualityComparer previous, ICondition<object> floating)
	{
		_previous = previous;
		_floating = floating;
	}

	public new bool Equals(object? x, object? y)
		=> _previous.Equals(x, y) || (IsEnumerable(x) && IsEnumerable(y)
			                              ? AreSequencesEqual((IEnumerable)x!, (IEnumerable)y!)
			                              : AreElementsEqual(x, y));

	public int GetHashCode(object obj) => _previous.GetHashCode(obj);

	static bool IsEnumerable(object? obj) => obj is IEnumerable and not string;

	bool AreSequencesEqual(IEnumerable ex, IEnumerable ey)
	{
		var enumX = ex.GetEnumerator();
		var enumY = ey.GetEnumerator();
		try
		{
			while (true)
			{
				var hasNextX = enumX.MoveNext();
				var hasNextY = enumY.MoveNext();

				if (hasNextX != hasNextY) return false;
				if (!hasNextX) return true;

				if (!AreElementsEqual(enumX.Current, enumY.Current))
					return false;
			}
		}
		finally
		{
			if (enumX is IDisposable d1) d1.Dispose();
			if (enumY is IDisposable d2) d2.Dispose();
		}
	}

	bool AreElementsEqual(object? x, object? y)
	{
		if (ReferenceEquals(x, y)) return true;
		if (x is null || y is null) return false;

		if (x is IConvertible && y is IConvertible && x.GetType() != y.GetType())
		{
			try
			{
				return _floating.Get(x) || _floating.Get(y)
					       ? Convert.ToDouble(x, CultureInfo.InvariantCulture)
					                .Equals(Convert.ToDouble(y, CultureInfo.InvariantCulture))
					       : Convert.ToInt64(x, CultureInfo.InvariantCulture)
					                .Equals(Convert.ToInt64(y, CultureInfo.InvariantCulture));
			}
			catch
			{
				// Fallback on error
			}
		}

		return _previous.Equals(x, y);
	}
}