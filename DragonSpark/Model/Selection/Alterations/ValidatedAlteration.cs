using System;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Selection.Alterations
{
	public class ValidatedAlteration<T> : Validated<T, T>
	{
		public ValidatedAlteration(ICondition<T> condition, ISelect<T, T> select) : base(condition, select) {}

		public ValidatedAlteration(ICondition<T> condition, ISelect<T, T> select, ISelect<T, T> fallback) :
			base(condition, select, fallback) {}

		public ValidatedAlteration(Func<T, bool> specification, Func<T, T> source) : base(specification, source) {}

		public ValidatedAlteration(Func<T, bool> specification, Func<T, T> source, Func<T, T> fallback) :
			base(specification, source, fallback) {}
	}
}