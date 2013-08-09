using System;

namespace DragonSpark.Objects
{
	public class ObjectCreatingEventArgs : ObjectCreatingEventArgs<object,object>
	{
		public ObjectCreatingEventArgs( object source ) : base( source )
		{}
	}

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