using System;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Testing.Framework
{
	public static class TestContext
	{
		public static Type GetCurrentHostType()
		{
			var result= Current.Transform( x => AppDomain.CurrentDomain.GetAssemblies().Select( y => y.GetType( x.FullyQualifiedTestClassName ) ) ).NotNull().FirstOrDefault();
			return result;
		}

		public static void Assign( Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context )
		{
			Current = context;
		}

		public static Microsoft.VisualStudio.TestTools.UnitTesting.TestContext Current { get; private set; }
	}
}