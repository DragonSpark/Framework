using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using System;
using System.Collections.Generic;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.Extensions
{
	public class WeakReferenceListExtensionsTests
	{
		[Theory, AutoData]
		void CheckWith()
		{
			var list = new List<WeakReference<Class>>();
			var count = 0;

			new Action(() => 
			{
				var item = new Class();
				list.CheckWith( item, @class => ++count );
				Assert.Equal( 1, count );
				list.CheckWith( item, @class => ++count );
				Assert.Equal( 1, count );
			})();

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.Equal( 0, list.AliveOnly().Count );

		}
	}
}