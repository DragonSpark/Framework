using DragonSpark.Application.Model.Sequences;
using DragonSpark.Model;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Model;

public class OptionCollection<T> : SelectedCollection<Option<T>>
{
	public OptionCollection() : this(Empty.Array<Option<T>>()) {}

	public OptionCollection(IEnumerable<Option<T>> list) : base(list) {}
}