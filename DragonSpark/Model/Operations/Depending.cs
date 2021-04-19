using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Depending<T> : Selecting<T, bool>, IDepending<T>
	{
		public Depending(ISelect<T, ValueTask<bool>> select) : base(select) {}

		public Depending(Func<T, ValueTask<bool>> select) : base(select) {}
	}
}