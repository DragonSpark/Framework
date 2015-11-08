using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonSpark.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing
{
	public interface IFoo
	{

	}

	public class Foo : IFoo
	{
	}

	[TestClass]
	public class ThreadLocalStorageFixture
	{
		[TestMethod]
		public void Push()
		{
			Foo foo = new Foo();
			IDisposable disposable = ThreadLocalStorage.Push<IFoo>(foo);

			Assert.IsNotNull(disposable);
		}

		[TestMethod]
		public void Peek()
		{
			Foo foo = new Foo();
			ThreadLocalStorage.Push<IFoo>(foo);

			Assert.AreSame(foo, ThreadLocalStorage.Peek<IFoo>());
		}

		[TestMethod]
		public void Dispose()
		{
			Foo foo = new Foo();
			IDisposable disposable = ThreadLocalStorage.Push<IFoo>(foo);
			disposable.Dispose();

			Assert.IsNull(ThreadLocalStorage.Peek<IFoo>());
		}
	}
}