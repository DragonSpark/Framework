using System;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Resources;
using DragonSpark.Testing.TestObjects.Synchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Objects.Synchronization
{
	[TestClass]
	public class ComplexPropertyMappingTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyCorrectMappings()
		{
			var source = new EntityView
			             	{
			             		ComplexPropertyView = new EntityViewComplexProperty
			             		                      	{
			             		                      		ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ),
			             		                      		Name = "New Name Yay!"
			             		                      	}
			             	};

			var target = new TestObjects.Synchronization.Entity
			             	{
			             		ComplexProperty = new EntityComplexProperty { ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ) }
			             	};

			var mappings = new ComplexPropertyMappings();
			mappings.Synchronize( source, target );

			Assert.AreEqual( source.ComplexPropertyView.Name, target.ComplexProperty.Name );
		}
	}
}