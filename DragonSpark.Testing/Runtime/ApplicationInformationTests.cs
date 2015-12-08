using System;
using System.Diagnostics;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class ApplicationInformationTests
	{
		[Theory, Test, SetupAutoData]
		public void Create( [Located]ApplicationInformation sut )
		{
			Assert.NotNull( sut.AssemblyInformation );
			Assert.Equal( DateTimeOffset.Parse( "2/1/2016" ), sut.DeploymentDate.GetValueOrDefault() );
			Assert.Equal( "http://framework.dragonspark.us/testing", sut.CompanyUri.ToString() );
			var assembly = GetType().Assembly;
			Assert.Equal( assembly.FromMetadata<AssemblyTitleAttribute, string>( attribute => attribute.Title ), sut.AssemblyInformation.Title );
			Assert.Equal( assembly.FromMetadata<AssemblyCompanyAttribute, string>( attribute => attribute.Company ), sut.AssemblyInformation.Company );
			Assert.Equal( assembly.FromMetadata<AssemblyCopyrightAttribute, string>( attribute => attribute.Copyright ), sut.AssemblyInformation.Copyright );
			Assert.Equal( assembly.FromMetadata<DebuggableAttribute, string>( attribute => "DEBUG" ), sut.AssemblyInformation.Configuration );
			Assert.Equal( assembly.FromMetadata<AssemblyDescriptionAttribute, string>( attribute => attribute.Description ), sut.AssemblyInformation.Description );
			Assert.Equal( assembly.FromMetadata<AssemblyProductAttribute, string>( attribute => attribute.Product ), sut.AssemblyInformation.Product );
			Assert.Equal( assembly.GetName().Version, sut.AssemblyInformation.Version );
		}
	}
}