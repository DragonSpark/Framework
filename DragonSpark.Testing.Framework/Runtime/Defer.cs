using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DragonSpark.Testing.Framework.Runtime
{
	public static class Defer
	{
		public static Task Run( Action<Task> action, [Optional]object context )
		{
			var task = context as Task;
			if ( task != null )
			{
				return task.ContinueWith( action );
			}
			action( Task.CompletedTask );
			return null;
		}
	}
}