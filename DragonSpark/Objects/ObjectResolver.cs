using System;
using DragonSpark.Extensions;

namespace DragonSpark.Objects
{
	public class ObjectResolver<TObjectType> : IObjectResolver where TObjectType : class
	{
		readonly ILocator finder;
		readonly IFactory creator;
		public event EventHandler<ObjectResolvedEventArgs> Resolved = delegate { };

		public ObjectResolver( ILocator finder = null, IFactory creator = null )
		{
			this.finder = finder ?? new Locator();
			this.creator = creator ?? new Factory<TObjectType>();
		}

		public TObjectType Resolve( object source )
		{
			var item = finder.Find( source ) ?? creator.Create( typeof(TObjectType), source );
			var result = item.To<TObjectType>();
			Resolved( this, new ObjectResolvedEventArgs( source, result ) );
			return result;
		}

		object IObjectResolver.Resolve( object source )
		{
			var result = Resolve( source );
			return result;
		}
	}
}