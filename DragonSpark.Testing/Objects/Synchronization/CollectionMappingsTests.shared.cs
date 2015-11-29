using System;
using System.Collections.Generic;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Resources;
using DragonSpark.Testing.TestObjects.Synchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Objects.Synchronization
{
	[TestClass]
	public class CollectionMappingsTests
	{
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
			var mappings = new CollectionMappings();
			mappings.Synchronize( source, target );

			Assert.AreEqual( source.ChildViews.Count, target.Children.Count );
		}
	}
}