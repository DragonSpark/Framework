using System;

namespace DragonSpark.Objects
{
	public class ObjectCreatedEventArgs<TObject> : EventArgs
	{
		readonly TObject result;

		public ObjectCreatedEventArgs( TObject result )
		{
			this.result = result;
		}

		public TObject Result
		{
			get { return result; }
		}
	}
}