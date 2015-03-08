using System;

namespace DragonSpark.Application.Communication
{
	public class CallbackResultEventArgs : EventArgs
	{
		public CallbackResultEventArgs( object result )
		{
			Result = result;
		}

		public object Result { get; private set; }
	}
}