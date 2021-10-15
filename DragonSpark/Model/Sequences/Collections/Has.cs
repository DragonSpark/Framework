using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Model.Sequences.Collections;

public class Has<T> : Condition<T>, IActivateUsing<ICollection<T>>, IActivateUsing<IEnumerable<T>>
{
	public Has(ICollection<T> source) : base(source.Contains) {}

	public Has(IEnumerable<T> source) : base(source.Contains) {}
}