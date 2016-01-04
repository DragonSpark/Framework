using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

		[Theory, ConfiguredMoqAutoData]
		public void Constructor( BuildUp sut )
		{
			var aspects = sut.ProvideAspects( typeof(BuildTarget) ).Fixed();
			Assert.Equal( 1, aspects.Length );

			var elements = aspects.Select( instance => instance.TargetElement ).Fixed();
			var constructor = elements.Only();
			Assert.IsAssignableFrom<ConstructorInfo>( constructor );
		}

		[Theory, ConfiguredMoqAutoData]
		public void Method( BuildUp sut )
		{
			var aspects = sut.ProvideAspects( typeof(BuildTarget).GetMethod( nameof(BuildTarget.Call) ) ).Fixed();
			Assert.Equal( 1, aspects.Length );

			var elements = aspects.Select( instance => instance.TargetElement ).Fixed();
			var element = elements.Only();
			Assert.IsAssignableFrom<MethodInfo>( element );
		}

		[BuildUp]
		public class BuildTarget
		{
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