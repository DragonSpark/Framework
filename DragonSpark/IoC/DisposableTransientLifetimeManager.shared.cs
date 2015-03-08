using System;
using System.Collections.Generic;
using DragonSpark.Extensions;

namespace DragonSpark.IoC
{
	public sealed class DisposableTransientLifetimeManager : Microsoft.Practices.Unity.TransientLifetimeManager, IDisposable
	{
		readonly IList<IDisposable> list = new List<IDisposable>();
		public override void SetValue(object newValue)
		{
			newValue.As<IDisposable>( list.Add );
		}

		public void Dispose()
		{
			list.Apply( x => x.Dispose() );
		}
	}
}