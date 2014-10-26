using System;
using DragonSpark.Configuration;
using DragonSpark.Extensions;

namespace DragonSpark.Objects.Configuration
{
	public class ObjectResolver : ISingleton<IObjectResolver>
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ItemType { get; set; }

		public ObjectFinderBase ObjectFinder { get; set; }

		public SingletonBase Singleton { get; set; }

		public IObjectResolver Instance
		{
			get { return instance ?? ( instance = Create() ); }
		}	IObjectResolver instance;

		protected virtual IObjectResolver Create()
		{
			var type = typeof(ObjectResolver<>).MakeGenericType( ItemType );
			var finder = ObjectFinder.Transform( x => x.Instance );
			var creator = Singleton.Transform( x => x.Instance );
			var result = Activator.CreateInstance( type, finder, creator ).To<IObjectResolver>();
			return result;
		}
	}
}
