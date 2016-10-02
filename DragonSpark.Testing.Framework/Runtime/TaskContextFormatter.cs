using System;
using System.Runtime.InteropServices;

namespace DragonSpark.Testing.Framework.Runtime
{
	public sealed class TaskContextFormatter : IFormattable
	{
		readonly TaskContext context;
		public TaskContextFormatter( TaskContext context )
		{
			this.context = context;
		}

		public string ToString( [Optional]string format, [Optional]IFormatProvider formatProvider ) => context.Origin.ToString();
	}
}