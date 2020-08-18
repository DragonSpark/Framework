using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Depending<T> : Selecting<T, bool>, IDepending<T>
	{
		public Depending([NotNull] ISelect<T, ValueTask<bool>> select) : base(select) {}

		public Depending([NotNull] Func<T, ValueTask<bool>> select) : base(select) {}
	}
}