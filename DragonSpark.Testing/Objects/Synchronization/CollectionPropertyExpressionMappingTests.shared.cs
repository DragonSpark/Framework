using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Objects.Synchronization;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects.Synchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DragonSpark.Extensions;

namespace DragonSpark.Testing.Objects.Synchronization
{
	[TestClass]
	public class CollectionPropertyExpressionMappingTests
	{
		readonly SynchronizationKey PolicyKey = new SynchronizationKey( typeof(EntityViewWithCollectionProperty), typeof(EntityWithCollectionProperty) );

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyChildrenCountMappedProperly()
		{
			var source = new EntityViewWithCollectionProperty
			             	{
			             		ChildViews = new List<EntityViewChild>
			             		             	{
			             		             		new EntityViewChild
			             		             			{ ID = new Guid( "{69654347-1FBD-43b5-8311-6764C66D8BAF}" ), Name = "Child One" },
			             		             		new EntityViewChild
			             		             			{ ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ), Name = "Child Two" }
			             		             	}
			             	};

			var target = new EntityWithCollectionProperty
			             	{
			             		Children = new List<EntityChild>
			             		           	{
			             		           		new EntityChild { ID = new Guid( "{79542AD4-598F-476a-B98A-27499F57AD8F}" ) }
			             		           	}
			             	};

			var cache = new List<EntityChild>
			            	{
			            		new EntityChild { ID = new Guid( "{69654347-1FBD-43b5-8311-6764C66D8BAF}" ), Name = "Current Name" },
								new EntityChild { ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ), Name = "Current Child Two" },
								new EntityChild { ID = new Guid( "{79542AD4-598F-476a-B98A-27499F57AD8F}" ), Name = "Current Child Three" }
			            	};
			var finder = new StaticCacheLocator<EntityViewChild, EntityChild>( cache );
			var creator = new Factory<EntityViewChild, EntityChild>();
			var resolver = new ObjectResolver<EntityChild>( finder, creator );
			var childKey = new SynchronizationKey( typeof(EntityViewChild), typeof(EntityChild) );
			var collection = new ListSynchronizationContext( resolver, "ChildViews", "Children" );
			var policies = new[]
			               	{
			               		new SynchronizationPolicy( PolicyKey, new SynchronizationExpressionResolver( collection.ToEnumerable<ISynchronizationContext>() ) ),
								new SynchronizationPolicy( childKey, new[] { new SimilarProperties() } ), 
			               	};
			var container = new SynchronizationContainer( policies );
			finder.Finding += ( sender, args ) => args.Result = ( from item in args.List where item.ID == args.Source.ID select item ).FirstOrDefault();
			container.Synchronize( source, target );

			Assert.AreEqual( source.ChildViews.Count, target.Children.Count );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyChildrenPropertiesMappedProperly()
		{
			var source = new EntityViewWithCollectionProperty
			             	{
			             		ChildViews = new List<EntityViewChild>
			             		             	{
			             		             		new EntityViewChild
			             		             			{ ID = new Guid( "{69654347-1FBD-43b5-8311-6764C66D8BAF}" ), Name = "Child One" },
			             		             		new EntityViewChild
			             		             			{ ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ), Name = "Child Two" }
			             		             	}
			             	};

			var target = new EntityWithCollectionProperty
			             	{
			             		Children = new List<EntityChild>
			             		           	{
			             		           		new EntityChild { ID = new Guid( "{79542AD4-598F-476a-B98A-27499F57AD8F}" ) }
			             		           	}
			             	};

			var cache = new List<EntityChild>
			            	{
			            		new EntityChild { ID = new Guid( "{69654347-1FBD-43b5-8311-6764C66D8BAF}" ), Name = "Current Name" },
								new EntityChild { ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ), Name = "Current Child Two" },
								new EntityChild { ID = new Guid( "{79542AD4-598F-476a-B98A-27499F57AD8F}" ), Name = "Current Child Three" }
			            	};

			var finder = new StaticCacheLocator<EntityViewChild, EntityChild>( cache );
			finder.Finding += ( sender, args ) => args.Result = ( from item in args.List where item.ID == args.Source.ID select item ).FirstOrDefault();
			var creator = new Factory<EntityViewChild, EntityChild>();
			var resolver = new ObjectResolver<EntityChild>( finder, creator );
			var childKey = new SynchronizationKey( typeof(EntityViewChild), typeof(EntityChild) );
			var collection = new ListSynchronizationContext( resolver, "ChildViews", "Children" );
			var policies = new[]
			               	{
			               		new SynchronizationPolicy( PolicyKey, new SynchronizationExpressionResolver( collection.ToEnumerable<ISynchronizationContext>() ) ),
								new SynchronizationPolicy( childKey, new[] { new SimilarProperties() } ), 
			               	};
			var container = new SynchronizationContainer( policies );
			container.Synchronize( source, target );

			for ( var i = 0; i < target.Children.Count; i++ )
			{
				Assert.AreEqual( source.ChildViews[i].ID, target.Children[i].ID );
				Assert.AreEqual( source.ChildViews[i].Name, target.Children[i].Name );
			}
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyEntityChildrenCountMappedProperly()
		{
			var source = new EntityWithCollectionProperty
			             	{
			             		Children = new List<EntityChild>
			             		           	{
			             		           		new EntityChild { ID = new Guid( "{79542AD4-598F-476a-B98A-27499F57AD8F}" ) }
			             		           	}
			             	};

			var target = new EntityViewWithCollectionProperty
			             	{
			             		ChildViews = new List<EntityViewChild>
			             		             	{
			             		             		new EntityViewChild
			             		             			{ ID = new Guid( "{69654347-1FBD-43b5-8311-6764C66D8BAF}" ), Name = "Child One" },
			             		             		new EntityViewChild
			             		             			{ ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ), Name = "Child Two" }
			             		             	}
			             	};

			var cache = new List<EntityViewChild>
			            	{
			            		new EntityViewChild { ID = new Guid( "{69654347-1FBD-43b5-8311-6764C66D8BAF}" ), Name = "Current Name" },
								new EntityViewChild { ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ), Name = "Current Child Two" },
								new EntityViewChild { ID = new Guid( "{79542AD4-598F-476a-B98A-27499F57AD8F}" ), Name = "Current Child Three" }
			            	};

			var finder = new StaticCacheLocator<EntityChild, EntityViewChild>( cache );
			finder.Finding += ( sender, args ) => args.Result = ( from item in args.List where item.ID == args.Source.ID select item ).FirstOrDefault();
			var creator = new Factory<EntityViewChild, EntityChild>();
			var resolver = new ObjectResolver<EntityViewChild>( finder, creator );
			var childKey = new SynchronizationKey( typeof(EntityViewChild), typeof(EntityChild) );
			var policies = new[]
			               	{
			               		new SynchronizationPolicy( PolicyKey, new SynchronizationExpressionResolver( new ListSynchronizationContext( resolver, "ChildViews", "Children" ).ToEnumerable<ISynchronizationContext>() ) ),
								new SynchronizationPolicy( childKey, new[] { new SimilarProperties() } ), 
			               	};
			var container = new SynchronizationContainer( policies );
			container.Synchronize( source, target );

			Assert.AreEqual( source.Children.Count, target.ChildViews.Count );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyEntityChildrenPropertiesMappedProperly()
		{
			var source = new EntityWithCollectionProperty
			             	{
			             		Children = new List<EntityChild>
			             		           	{
			             		           		new EntityChild { ID = new Guid( "{79542AD4-598F-476a-B98A-27499F57AD8F}" ) }
			             		           	}
			             	};

			var target = new EntityViewWithCollectionProperty
			             	{
			             		ChildViews = new List<EntityViewChild>
			             		             	{
			             		             		new EntityViewChild
			             		             			{ ID = new Guid( "{69654347-1FBD-43b5-8311-6764C66D8BAF}" ), Name = "Child One" },
			             		             		new EntityViewChild
			             		             			{ ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ), Name = "Child Two" }
			             		             	}
			             	};

			var cache = new List<EntityViewChild>
			            	{
			            		new EntityViewChild { ID = new Guid( "{69654347-1FBD-43b5-8311-6764C66D8BAF}" ), Name = "Current Name" },
								new EntityViewChild { ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ), Name = "Current Child Two" },
								new EntityViewChild { ID = new Guid( "{79542AD4-598F-476a-B98A-27499F57AD8F}" ), Name = "Current Child Three" }
			            	};

			var finder = new StaticCacheLocator<EntityChild, EntityViewChild>( cache );
			finder.Finding += ( sender, args ) => args.Result = ( from item in args.List where item.ID == args.Source.ID select item ).FirstOrDefault();
			var creator = new Factory<EntityViewChild, EntityChild>();
			var resolver = new ObjectResolver<EntityViewChild>( finder, creator );
			var childKey = new SynchronizationKey( typeof(EntityViewChild), typeof(EntityChild) );
			var policies = new[]
			               	{
			               		new SynchronizationPolicy( PolicyKey, new SynchronizationExpressionResolver( new ListSynchronizationContext( resolver, "ChildViews", "Children" ).ToEnumerable<ISynchronizationContext>() ) ),
								new SynchronizationPolicy( childKey, new[] { new SimilarProperties() } ), 
			               	};
			var container = new SynchronizationContainer( policies );
			container.Synchronize( source, target );


			for ( var i = 0; i < target.ChildViews.Count; i++ )
			{
				Assert.AreEqual( source.Children[i].ID, target.ChildViews[i].ID );
				Assert.AreEqual( source.Children[i].Name, target.ChildViews[i].Name );
			}
		}
	}
}