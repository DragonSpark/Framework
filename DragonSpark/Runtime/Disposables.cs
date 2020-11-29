using DragonSpark.Model.Sequences.Collections;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
			GC.SuppressFinalize(this);
			var count = _collection.Count;
			for (var i = 0; i < count; i++)
			{
				_collection[i].Dispose();
			}

			_collection.Clear();
		}
	}
}