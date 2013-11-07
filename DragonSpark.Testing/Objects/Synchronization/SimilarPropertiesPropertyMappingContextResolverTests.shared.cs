using System;
using System.Collections.Generic;
using DragonSpark.Objects.Synchronization;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects.Synchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Objects.Synchronization
{
	[TestClass]
	public class SimilarPropertiesPropertyMappingContextResolverTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyBasePropertiesMapped()
		{
			var source = new EntityView { ID = Guid.NewGuid(), Name = "Entity View", Date = "06/07/08", CommentsField = "Heheh" };
			var target = new TestObjects.Synchronization.Entity();

			var key = new SynchronizationKey( typeof(object), typeof(object) );
			var policy = new SynchronizationPolicy( key, new[] { new SimilarProperties() } );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			container.Synchronize( source, target, true );

			Assert.AreEqual( source.ID, target.ID );
			Assert.AreEqual( source.Name, target.Name );
			Assert.AreEqual( DateTime.Parse( source.Date ), target.Date );
			Assert.AreNotEqual( "Heheh", target.Comments );
		}


		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifySimilarPropertiesMappedProperlyFromEntityViewToEntity()
		{
			var source = new EntityView { ID = Guid.NewGuid(), Name = "Entity View", Date = "06/07/08", CommentsField = "Heheh" };
			var target = new TestObjects.Synchronization.Entity();

			var key = new SynchronizationKey( source.GetType(), target.GetType() );
			var policy = new SynchronizationPolicy( key, new[] { new SimilarProperties() } );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			container.Synchronize( source, target );

			Assert.AreEqual( source.ID, target.ID );
			Assert.AreEqual( source.Name, target.Name );
			Assert.AreEqual( DateTime.Parse( source.Date ), target.Date );
			Assert.AreNotEqual( "Heheh", target.Comments );
		}
        
#if !SILVERLIGHT
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifySimilarPropertiesMappedProperlyFromEntityViewToEntityFromAWrapperContext()
		{
			
			var source = new {
			             	View = new EntityView { ID = Guid.NewGuid(), Name = "Entity View", Date = "06/07/08", CommentsField = "Heheh" }
							};
			var target = new TestObjects.Synchronization.Entity();

			var key = new SynchronizationKey( source.GetType(), target.GetType() );
			var policy = new SynchronizationPolicy( key, new[] { new SimilarProperties( "View", null ) } );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			container.Synchronize( source, target );

			Assert.AreEqual( source.View.ID, target.ID );
			Assert.AreEqual( source.View.Name, target.Name );
			Assert.AreEqual( DateTime.Parse( source.View.Date ), target.Date );
			Assert.AreNotEqual( "Heheh", target.Comments );
		}
#endif

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifySimilarPropertiesMappedProperlyFromEntityToEntityView()
		{
			var source = new TestObjects.Synchronization.Entity { ID = Guid.NewGuid(), Name = "Entity View", Date = DateTime.Parse( "06/07/08" ), Comments = "Heheh" };
			var target = new EntityView();

			var key = new SynchronizationKey( typeof(EntityView), typeof(TestObjects.Synchronization.Entity) );
			var policy = new SynchronizationPolicy( key, new[] { new SimilarProperties() } );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			container.Synchronize( source, target );

			Assert.AreEqual( source.ID, target.ID );
			Assert.AreEqual( source.Name, target.Name );
			var actual = DateTime.Parse( target.Date ).Date.ToShortDateString();
			Assert.AreEqual( "6/7/2008", actual );
			Assert.AreNotEqual( source.Comments, target.CommentsField );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyIgnoredPropertyDidNotMapFromEntityViewToEntity()
		{
			var expected = "This should be ignored during mapping";
			var source = new EntityView { ID = Guid.NewGuid(), Name = "Entity View", Date = "06/07/08", CommentsField = "Heheh" };
			var target = new TestObjects.Synchronization.Entity { IgnoreDisplayName = expected };

			var key = new SynchronizationKey( source.GetType(), target.GetType() );
			var policy = new SynchronizationPolicy( key, new[] { new SimilarProperties( new[] { "IgnoreDisplayName" } ) } );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			container.Synchronize( source, target );

			Assert.AreEqual( expected, target.IgnoreDisplayName );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyIgnoredPropertyDidNotMapFromEntityToEntityView()
		{
			var expected = "Ignore me View";
			var source = new TestObjects.Synchronization.Entity { ID = Guid.NewGuid(), Name = "Entity View", Date = DateTime.Parse( "06/07/08" ), Comments = "Heheh", IgnoreDisplayName = "This is a view display name." };
			var target = new EntityView { IgnoreDisplayName = expected };

			var key = new SynchronizationKey( typeof(EntityView), typeof(TestObjects.Synchronization.Entity) );
			var policy = new SynchronizationPolicy( key, new[] { new SimilarProperties( new[] { "IgnoreDisplayName" } ) } );
			var policies = new [] { policy };

			var container = new SynchronizationContainer( policies );
			container.Synchronize( source, target );

			Assert.AreEqual( expected, target.IgnoreDisplayName );
		}
	}
}