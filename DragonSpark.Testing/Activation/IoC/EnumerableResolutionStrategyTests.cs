using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.Activation.IoC
{
	public class EnumerableResolutionStrategyTests
	{
		[Theory, Test, SetupAutoData]
		void Resolve( [Located]Interfaces sut )
		{
			Assert.NotNull( sut.Items.FirstOrDefaultOfType<Item>() );
			Assert.NotNull( sut.Items.FirstOrDefaultOfType<AnotherItem>() );
			Assert.NotNull( sut.Items.FirstOrDefaultOfType<YetAnotherItem>() );
		}

		class Interfaces
		{
			public Interfaces( IEnumerable<IItem> items )
			{
				Items = items;
			}

			public IEnumerable<IItem> Items { get; }
		}

		public interface IItem
		{}

		public class Item : IItem
		{}

		[DragonSpark.Setup.Registration.Register( "AnotherItem" )]
		public class AnotherItem : IItem
		{}

		[DragonSpark.Setup.Registration.Register( "YetAnotherItem" )]
		public class YetAnotherItem : IItem
		{}
	}
}