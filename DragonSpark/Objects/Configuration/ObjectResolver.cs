using System;
using DragonSpark.Configuration;
using DragonSpark.Extensions;

namespace DragonSpark.Objects.Configuration
{
	public class ObjectResolver : IInstanceSource<IObjectResolver>
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ItemType { get; set; }

		public ObjectFinderBase ObjectFinder { get; set; }

		public InstanceSourceBase InstanceSource { get; set; }

		public IObjectResolver Instance
		{
			get { return instance ?? ( instance = Create() ); }
		}	IObjectResolver instance;

		protected virtual IObjectResolver Create()
		{
			var type = typeof(ObjectResolver<>).MakeGenericType( ItemType );
			var finder = ObjectFinder.Transform( x => x.Instance );
			var creator = InstanceSource.Transform( x => x.Instance );
			var result = Activator.CreateInstance( type, finder, creator ).To<IObjectResolver>();
			return result;
		}
	}
}
