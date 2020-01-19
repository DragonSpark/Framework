using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections
{
	public class NotHave<T> : InverseCondition<T>, IActivateUsing<ICollection<T>>, IActivateUsing<IEnumerable<T>>
	{
		public NotHave(ICollection<T> source) : base(new Has<T>(source)) {}

		public NotHave(IEnumerable<T> source) : base(new Has<T>(source)) {}
	}
}