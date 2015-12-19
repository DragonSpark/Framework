using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using System.ComponentModel;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class BuildUpTests
	{
		[Fact]
		public void Build()
		{
			var sut = new BuildTarget();
			Assert.True( sut.Boolean );
			sut.Boolean = false;

			Assert.Null( sut.Value );
			sut.Call();
			Assert.False( sut.Boolean );
			Assert.Same( SkipFirstCallProvider.Instance.Item, sut.Value );
		}

		[BuildUp]
		public class BuildTarget
		{
			public BuildTarget()
			{}

			[DefaultValue( true )]
			public bool Boolean { get; set; }

			[SkipFirstCallValue]
			public object Value { get; set; }

			[BuildUp]
			public void Call()
			{}
		}

		public class SkipFirstCallValue : DefaultValueBase
		{
			public SkipFirstCallValue() : base( () => SkipFirstCallProvider.Instance )
			{}
		}

		public class SkipFirstCallProvider : IDefaultValueProvider
		{
			public static SkipFirstCallProvider Instance { get; } = new SkipFirstCallProvider();

			readonly public object Item = new object();

			readonly ConditionMonitor monitor = new ConditionMonitor();

			public object GetValue( DefaultValueParameter parameter )
			{
				return monitor.Apply() ? null : Item;
			}
		}
	}
}