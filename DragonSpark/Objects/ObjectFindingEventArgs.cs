using System;
using System.Collections.Generic;

namespace DragonSpark.Objects
{
	public class ObjectFindingEventArgs<TSource,TObject> : EventArgs
	{
		readonly IEnumerable<TObject> list;
		readonly TSource source;

		public ObjectFindingEventArgs( IEnumerable<TObject> list, TSource source )
		{
			this.list = list;
			this.source = source;
		}

		public TSource Source
		{
			get { return source; }
		}

		public IEnumerable<TObject> List
		{
			get { return list; }
		}

		public TObject Result { get; set; }
	}
}