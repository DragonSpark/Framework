using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing.TestObjects.Synchronization
{
	public class EntityComplexPropertyFinder : StaticCacheLocator<EntityViewComplexProperty,EntityComplexProperty>
	{
		readonly static List<EntityComplexProperty> cache = new List<EntityComplexProperty>
		                                          	{
		                                          		new EntityComplexProperty { ID = new Guid( "{69654347-1FBD-43b5-8311-6764C66D8BAF}" ), Name = "Current Name" },
		                                          		new EntityComplexProperty { ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ), Name = "Current Complex Property Two" },
		                                          		new EntityComplexProperty { ID = new Guid( "{79542AD4-598F-476a-B98A-27499F57AD8F}" ), Name = "Current Complex Property Three" }
		                                          	};
		public EntityComplexPropertyFinder() : base( cache )
		{
			Finding += EntityChildFinder_Finding;
		}

		static void EntityChildFinder_Finding(object sender, ObjectFindingEventArgs<EntityViewComplexProperty, EntityComplexProperty> e )
		{
			var result = ( from item in e.List where item.ID == e.Source.ID select item ).FirstOrDefault();
			e.Result = result;
		}
	}
}