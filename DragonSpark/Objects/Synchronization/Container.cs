using System.Reflection;

namespace DragonSpark.Objects.Synchronization
{
	class Container
	{
		static readonly internal PropertyInfo ContentsProperty = typeof(Container).GetProperty( "Contents" );

		readonly object contents;

		public Container( object contents )
		{
			this.contents = contents;
		}

		public object Contents
		{
			get { return contents; }
		}
	}
}
