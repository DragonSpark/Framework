using DragonSpark.Extensions;
using DragonSpark.TypeSystem.Metadata;
using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.TypeSystem.Metadata
{
	public class ParameterInfoAttributeProviderTests
	{
		[Fact]
		public void Verify()
		{
			var provider = Attributes.Get( typeof(Subject).GetConstructors().Only().GetParameters().Last() );
			Assert.True( provider.Contains(typeof(RequiredAttribute) ) );
			Assert.Single( provider.GetAttributes(typeof(RequiredAttribute) ) );
		}

		class Subject
		{
			[UsedImplicitly]
			public Subject( object first, [Required]int second ) {}
		}
	}
}