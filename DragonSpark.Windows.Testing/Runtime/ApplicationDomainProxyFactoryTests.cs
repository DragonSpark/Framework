using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.Runtime;
using JetBrains.Annotations;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ApplicationDomainProxyFactoryTests
	{
		[Theory, AutoData]
		public void Verify( string name, int number )
		{
			var created = ApplicationDomainProxyFactory<Subject>.Default.Get( name, number );
			Assert.Equal( name, created.Name );
			Assert.Equal( number, created.Number );
		}

		[UsedImplicitly, Serializable]
		sealed class Subject
		{
			public Subject( string name, int number )
			{
				Name = name;
				Number = number;
			}

			public string Name { get; }
			public int Number { get; }
		}
	}
}