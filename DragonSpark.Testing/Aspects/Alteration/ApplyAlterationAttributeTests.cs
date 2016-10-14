using DragonSpark.Aspects.Alteration;
using DragonSpark.Commands;
using DragonSpark.Sources.Parameterized;
using Xunit;

namespace DragonSpark.Testing.Aspects.Alteration
{
	public class ApplyAlterationAttributeTests
	{
		[Fact]
		public void Verify()
		{
			var sut = new Subject();
			Assert.Null( sut.PropertyName );
			Command.Default.Execute( sut );
			Assert.Equal( Alteration.ParameterPropertyName, sut.PropertyName );
		}

		[ApplyAlteration( typeof(Alteration) )]
		class Command : CommandBase<Subject>
		{
			public static Command Default { get; } = new Command();
			Command() {}

			public override void Execute( Subject parameter ) {}
		}
		
		class Subject
		{
			public string PropertyName { get; set; }
		}

		class Alteration : IAlteration<Subject>
		{
			public const string ParameterPropertyName = "Altered";

			public Subject Get( Subject parameter )
			{
				parameter.PropertyName = ParameterPropertyName;
				return parameter;
			}
		}
	}
}