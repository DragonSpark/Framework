using DragonSpark.Compose;
using DragonSpark.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace DragonSpark.Application.Model.Sequences;

public sealed class ObservedList<T> : ObservableCollection<T>
{
	readonly IList<T> _source;

	public ObservedList(IList<T> source) : base(source) => _source = source;

	// ReSharper disable once CognitiveComplexity
	protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		var @new = (e.NewItems?.Cast<T>() ?? Empty.Array<T>()).Only();
		if (@new is not null)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					_source.Add(@new);
					break;
				case NotifyCollectionChangedAction.Replace:
					_source[e.NewStartingIndex] = @new;
					break;
				case NotifyCollectionChangedAction.Move:
					_source.RemoveAt(e.OldStartingIndex);
					_source.Insert(e.NewStartingIndex, @new);
					break;
			}
		}
		else
		{
			var old = (e.OldItems?.Cast<T>() ?? Empty.Array<T>()).Only();
			if (old is not null)
			{
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Remove:
						_source.Remove(old);
						break;
				}
			}
		}
		base.OnCollectionChanged(e);
	}
}