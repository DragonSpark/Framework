using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Server.IoC
{
	public class RequestLifetimeManager : KeyBasedLifetimeManager
	{
		public RequestLifetimeManager( string name ) : base( name )
		{}

		public override object GetValue()
		{
			var result = ServerContext.Current.Transform( x => x.Items[ Key ] );
			return result;
		}

		public override void RemoveValue() 
		{ 
			ServerContext.Current.NotNull( x => x.Items.Remove( Key ) );
		} 

		public override void SetValue(object newValue) 
		{
			ServerContext.Current.NotNull( x => x.Items[ Key ] = newValue ); 
		}
	}
}