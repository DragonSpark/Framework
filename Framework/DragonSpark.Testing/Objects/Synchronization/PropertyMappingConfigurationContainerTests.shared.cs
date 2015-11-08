using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Resources;
using DragonSpark.Testing.TestObjects.Synchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Objects.Synchronization
{
	[TestClass]
	public class PropertyMappingConfigurationContainerTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyBasicDescriptionMappedCorrectly()
		{
			const string description = "Entity Name";
			var source = new EntityView { Description = description };

			var mappings = new SimilarMappings();
			
			var entity = mappings.Create<TestObjects.Synchronization.Entity>( source );
			Assert.AreEqual( description, entity.Description );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyExpressionMappedCorrectly()
		{
			const string comments = "Some randomish comments";
			var source = new EntityView { CommentsField = comments };

			/*Caliburn.Micro.IoC.GetInstance = ( type, key ) => Activator.CreateInstance( type );
			Caliburn.Micro.IoC.GetAllInstances = type => new[] { Activator.CreateInstance( type ) };*/

			var mappings = new ExpressionMappings();
			var entity = mappings.Create<TestObjects.Synchronization.Entity>( source );
			Assert.AreEqual( comments, entity.Comments );
		}
	}
}