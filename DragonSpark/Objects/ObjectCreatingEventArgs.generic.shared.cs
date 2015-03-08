using System;

namespace DragonSpark.Objects
{
	public class ObjectCreatingEventArgs<TSource,TResult> : EventArgs
	{
		readonly TSource source;

		public ObjectCreatingEventArgs( TSource source )
		{
			this.source = source;
		}

		public TSource Source
		{
			get { return source; }
		}

		public TResult Result { get; set; }
	}
}