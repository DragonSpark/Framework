using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Objects.Setup;
using DragonSpark.TypeSystem;
using System;
using System.Composition;
using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace DragonSpark.Windows.Testing.Setup
{
	[Trait( Traits.Category, Traits.Categories.Xaml ), ContainingTypeAndNested, FrameworkTypes, FormatterTypes, AdditionalTypes( typeof(ProgramSetup), typeof(Program) )]
	// ReSharper disable once TestFileNameWarning
	public class ProgramSetupTests
	{
		[Theory, AutoData, AdditionalTypes( typeof(AssemblyInformationSource), typeof(ApplicationAssembly) )]
		public void Create( [Service]ApplicationInformation sut )
		{
			Assert.NotNull( sut.AssemblyInformation );
			Assert.Equal( DateTimeOffset.Parse( "2/1/2016" ), sut.DeploymentDate.GetValueOrDefault() );
			Assert.Equal( "http://framework.dragonspark.us/testing", sut.CompanyUri.ToString() );
			var assembly = GetType().Assembly;
			Assert.Equal( assembly.From<AssemblyTitleAttribute, string>( attribute => attribute.Title ), sut.AssemblyInformation.Title );
			Assert.Equal( assembly.From<AssemblyCompanyAttribute, string>( attribute => attribute.Company ), sut.AssemblyInformation.Company );
			Assert.Equal( assembly.From<AssemblyCopyrightAttribute, string>( attribute => attribute.Copyright ), sut.AssemblyInformation.Copyright );
			Assert.Equal( assembly.From<DebuggableAttribute, string>( attribute => "DEBUG" ), sut.AssemblyInformation.Configuration );
			Assert.Equal( assembly.From<AssemblyDescriptionAttribute, string>( attribute => attribute.Description ), sut.AssemblyInformation.Description );
			Assert.Equal( assembly.From<AssemblyProductAttribute, string>( attribute => attribute.Product ), sut.AssemblyInformation.Product );
			Assert.Equal( assembly.GetName().Version, sut.AssemblyInformation.Version );
		}

		[Theory, AutoData]
		public void Run( [Service]Program sut )
		{
			Assert.True( sut.Ran, "Didn't Run" );
			Assert.Equal( GetType().GetMethod( nameof(Run) ), sut.Arguments.Method );
			Assert.Equal( 1, Counting.Default.Get( CountingTarget.Default.Get() ) );
		}
	}

	[Shared]
	public class Program : Program<AutoData>
	{
		public bool Ran { get; private set; }

		public AutoData Arguments { get; private set; }

		protected override void Run( AutoData arguments )
		{
			Ran = true;
			Arguments = arguments;
		}
	}
}