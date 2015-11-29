using System.Collections.Generic;

namespace DragonSpark.Testing.TestObjects.Synchronization
{
	public class EntityWithCollectionProperty
	{	
		public List<EntityChild> Children
		{
			get { return children ?? ( children = new List<EntityChild>() ); }
			set { children = value; }
		}	List<EntityChild> children;
	}
}