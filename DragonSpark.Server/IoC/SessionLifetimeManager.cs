using DragonSpark.IoC;

namespace DragonSpark.Server.IoC
{
	public class SessionLifetimeManager : KeyBasedLifetimeManager
	{
		public SessionLifetimeManager( string name ) : base( name )
		{}

		public override object GetValue()
		{
			var result = ServerContext.Current.Session[ Key ];
			return result;
		}

		public override void RemoveValue() 
		{ 
			ServerContext.Current.Session.Remove( Key );
		} 

		public override void SetValue(object newValue) 
		{ 
			ServerContext.Current.Session[ Key ] = newValue; 
		}
	}
}