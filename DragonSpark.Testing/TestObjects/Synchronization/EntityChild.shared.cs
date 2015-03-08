using System;

namespace DragonSpark.Testing.TestObjects.Synchronization
{
	public class EntityChild : IEquatable<EntityChild>
	{
		public Guid ID  { get; set; }
		public string Name { get; set; }

		public override bool Equals(object obj)
		{
			EntityChild other = obj as EntityChild;
			if ( other != null )
			{
				return ID == other.ID;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		bool IEquatable<EntityChild>.Equals( EntityChild other )
		{
			return ID == other.ID;
		}
	}
}