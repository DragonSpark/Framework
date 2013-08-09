using System;
using System.Collections.Generic;
using DragonSpark.Objects.Synchronization;
using DragonSpark.Testing.Objects.Synchronization;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects.Synchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DragonSpark.Extensions;

namespace DragonSpark.Testing.Objects.Synchronization
{
	/// <summary>
	/// Summary description for PropertyMappingContainerTests
	/// </summary>
	[TestClass]
	public class PropertyMappingContainerTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyBasicNameMappedCorrectly()
		{
			var description = "Entity Name";
			var source = new EntityView { Description = description };
			var target = new TestObjects.Synchronization.Entity();

			var key = new SynchronizationKey( source.GetType(), target.GetType() );
			var policy = new SynchronizationPolicy( key, new SimpleProperty( "Description" ) );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( description, target.Description );
			container.Synchronize( source, target );

			Assert.IsNotNull( target.Description );
			Assert.AreEqual( description, source.Description );
			Assert.AreEqual( description, target.Description );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyMirroredBasicNameMappedCorrectly()
		{
			var description = "Entity Name";
			var source = new TestObjects.Synchronization.Entity { Description = description };
			var target = new EntityView();
			
			var key = new SynchronizationKey( typeof(EntityView), typeof(TestObjects.Synchronization.Entity) );
			var policy = new SynchronizationPolicy( key, new SimpleProperty( "Description" ) );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( description, target.Description );
			container.Synchronize( source, target );

			Assert.IsNotNull( target.Description );
			Assert.AreEqual( description, source.Description );
			Assert.AreEqual( description, target.Description );
		}


		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyExpressionMappedCorrectly()
		{
			var comments = "These are my silly comments";
			var source = new EntityView { CommentsField = comments };
			var target = new TestObjects.Synchronization.Entity();

			var key = new SynchronizationKey( source.GetType(), target.GetType() );
			var collection = new SynchronizationExpressionResolver( new SynchronizationContext( "CommentsField", "Comments" ).ToEnumerable<ISynchronizationContext>() );
			var policy = new SynchronizationPolicy( key, collection );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( comments, target.Comments );
			container.Synchronize( source, target );

			Assert.IsNotNull( target.Comments );
			Assert.AreEqual( comments, source.CommentsField );
			Assert.AreEqual( comments, target.Comments );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyExpressionMappedCorrectlyMirrored()
		{
			var comments = "These are my silly comments";
			var source = new TestObjects.Synchronization.Entity() { Comments = comments };
			var target = new EntityView();

			var key = new SynchronizationKey( typeof(EntityView), typeof(TestObjects.Synchronization.Entity) );
			var collection = new SynchronizationExpressionResolver( new[]{ new SynchronizationContext( "CommentsField", "Comments" ) } );
			var policy = new SynchronizationPolicy( key, collection );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( comments, target.CommentsField );
			container.Synchronize( source, target );

			Assert.IsNotNull( target.CommentsField );
			Assert.AreEqual( comments, source.Comments );
			Assert.AreEqual( comments, target.CommentsField );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyReflectionPropertyMappedCorrectly()
		{
			var description = "Child Description";
			var source = new EntityView { ChildDescription = description };
			var target = new TestObjects.Synchronization.Entity();

			var key = new SynchronizationKey( source.GetType(), target.GetType() );
			var policy = new SynchronizationPolicy( key );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( description, target.Child.Description );
			container.Synchronize( source, target );

			Assert.IsNotNull( target.Child.Description );
			Assert.AreEqual( description, source.ChildDescription );
			Assert.AreEqual( description, target.Child.Description );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyMirroredReflectionPropertyMappedCorrectly()
		{
			var description = "Child Description";
			var source = new TestObjects.Synchronization.Entity { Child = { Description = description } };
			var target = new EntityView();

			var key = new SynchronizationKey( typeof(EntityView), typeof(TestObjects.Synchronization.Entity) );
			var policy = new SynchronizationPolicy( key );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( description, target.ChildDescription );
			container.Synchronize( source, target );

			Assert.IsNotNull( target.ChildDescription );
			Assert.AreEqual( description, source.Child.Description );
			Assert.AreEqual( description, target.ChildDescription );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyBaseClassesNotProcessed()
		{
			var description = "Entity Name";
			var source = new EntityView { Description = description };
			var target = new SuperEntity();

			var key = new SynchronizationKey( source.GetType(), typeof(TestObjects.Synchronization.Entity) );
			var collection = new SimpleProperty( "Description" );
			var policy = new SynchronizationPolicy( key, collection );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( description, target.Description );
			container.Synchronize( source, target );
			
			Assert.IsNull( target.Description );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyBaseClassesProcessed()
		{
			var description = "Entity Name";
			var source = new EntityView { Description = description };
			var target = new SuperEntity();

			var key = new SynchronizationKey( source.GetType(), typeof(TestObjects.Synchronization.Entity) );
			var collection = new SimpleProperty( "Description" );
			var policy = new SynchronizationPolicy( key, collection );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( description, target.Description );
			container.Synchronize( source, target, true );

			Assert.IsNotNull( target.Description );
			Assert.AreEqual( description, target.Description );
			Assert.AreEqual( source.Description, target.Description );
		}


		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyConversionMappedCorrectly()
		{
			var date = "06/07/08";
			var expected = DateTime.Parse( date );
			var source = new EntityView { Date = date };
			var target = new TestObjects.Synchronization.Entity();

			var key = new SynchronizationKey( source.GetType(), target.GetType() );
			var collection = new SimpleProperty( "Date" );
			var policy = new SynchronizationPolicy( key, collection );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( date, target.Date );
			container.Synchronize( source, target );

			Assert.AreEqual( date, source.Date );
			Assert.AreEqual( expected, target.Date );
		}
        
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyNamedPropertyKeyDoesNotMapWhenDifferentNameIsUsed()
		{
			var description = "Entity Description";
			var source = new EntityView { Description = description };
			var target = new TestObjects.Synchronization.Entity();

			var key = new SynchronizationKey( source.GetType(), target.GetType(), "Named" );
			var collection = new SimpleProperty( "Description" );
			var policy = new SynchronizationPolicy( key, collection );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( description, target.Description );
			container.Synchronize( source, target );

			Assert.IsNull( target.Description );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyNamedPropertyMapsWhenSameNameIsUsed()
		{
			var description = "Entity Description";
			var source = new EntityView { Description = description };
			var target = new TestObjects.Synchronization.Entity();

			var name = "Named";
			var key = new SynchronizationKey( source.GetType(), target.GetType(), name );
			var collection = new SimpleProperty( "Description" );
			var policy = new SynchronizationPolicy( key, collection );
			var policies = new List<ISynchronizationPolicy> { policy };

			var container = new SynchronizationContainer( policies );
			Assert.AreNotEqual( description, target.Description );
			container.Synchronize( source, target, name );

			Assert.IsNotNull( target.Description );
			Assert.AreEqual( description, source.Description );
			Assert.AreEqual( description, target.Description );
		}

	}
}
