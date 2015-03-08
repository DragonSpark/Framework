using System.Collections.Generic;

namespace DragonSpark.Testing.TestObjects
{
	public class DragonSparkObject
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public List<DragonSparkChild> Children
		{
			get { return children ?? ( children = new List<DragonSparkChild>() ); }
		}	List<DragonSparkChild> children;
	}
}
