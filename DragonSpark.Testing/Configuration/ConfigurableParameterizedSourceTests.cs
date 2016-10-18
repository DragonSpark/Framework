﻿using DragonSpark.Configuration;
using Xunit;

namespace DragonSpark.Testing.Configuration
{
	public class ConfigurableParameterizedSourceTests
	{
		[Fact]
		public void VerifyConfiguration()
		{
			const int parameter = 123;
			var before = Configurable.Default.Get( parameter );
			Assert.Equal( parameter + 6776, before );

			Configurable.Default.Configuration.Assign( () => i => i * 2 );

			const int answer = 42;
			var after = Configurable.Default.Get( answer );
			Assert.Equal( answer * 2, after );
		}

		sealed class Configurable : ConfigurableParameterizedSource<int, int>
		{
			public static Configurable Default { get; } = new Configurable();
			Configurable() : base( i => i + 6776 ) {}
		}
	}
}