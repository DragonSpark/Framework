using System;

namespace DragonSpark.Model.Selection.Alterations
{
	public class Alteration<T> : Select<T, T>, IAlteration<T>
	{
		public Alteration(ISelect<T, T> select) : this(select.Get) {}

		public Alteration(Func<T, T> alteration) : base(alteration) {}
	}
}