using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing.TestObjects.Synchronization
{
	public class EntityChildFinder : StaticCacheLocator<EntityViewChild,EntityChild>
	{
		readonly static List<EntityChild> cache = new List<EntityChild>
		                                          	{
		                                          		new EntityChild { ID = new Guid( "{69654347-1FBD-43b5-8311-6764C66D8BAF}" ), Name = "Current Name" },
		                                          		new EntityChild { ID = new Guid( "{D030FDAF-3661-47c0-ADB7-5DE48BD6CF2F}" ), Name = "Current Child Two" },
		                                          		new EntityChild { ID = new Guid( "{79542AD4-598F-476a-B98A-27499F57AD8F}" ), Name = "Current Child Three" }
		                                          	};
		public EntityChildFinder() : base( cache )
		{
			Finding += EntityChildFinder_Finding;
		}

		static void EntityChildFinder_Finding(object sender, ObjectFindingEventArgs<EntityViewChild, EntityChild> args )
		{
			var result = ( from item in args.List where item.ID == args.Source.ID select item ).FirstOrDefault();
			args.Result = result;
		}
	}
}