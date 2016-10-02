using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.TypeSystem;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing
{
	[Trait( Traits.Category, Traits.Categories.ServiceLocation )]
	public class FormatterTests
	{
		[Theory, AutoData, FrameworkTypes, FormatterTypes]
		public void MethodFormatsAsExpected( [Service]Formatter sut )
		{
			var method = MethodBase.GetCurrentMethod();
			var formatted = sut.Get( new FormatterParameter( method ) );
			Assert.IsType<string>( formatted );
			Assert.Equal( new MethodFormatter( method ).ToString(), formatted );
		}
	}
}