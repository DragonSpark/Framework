using DragonSpark.Extensions;
using System;

namespace DragonSpark.TypeSystem
{
	public static class Where<T>
	{
		public static Func<T, bool> Assigned => t => t.IsAssigned();

		public static Func<T, bool> Always => t => true;
	}
}