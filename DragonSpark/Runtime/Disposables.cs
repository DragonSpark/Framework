using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using DragonSpark.Model.Sequences.Collections;

namespace DragonSpark.Runtime
{
	public class Disposables : Membership<IDisposable>, IDisposable
	{
		readonly IList<IDisposable> _collection;

		[UsedImplicitly]
		public Disposables() : this(new Collection<IDisposable>()) {}

		public Disposables(IList<IDisposable> collection) : base(collection) => _collection = collection;

		public void Dispose()
		{
			var count = _collection.Count;
			for (var i = 0; i < count; i++)
			{
				_collection[i].Dispose();
			}

			_collection.Clear();
		}
	}
}