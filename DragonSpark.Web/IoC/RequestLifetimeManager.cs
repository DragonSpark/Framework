using System.Web;
using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Web.IoC
{
	public class RequestLifetimeManager : KeyBasedLifetimeManager
	{
		public RequestLifetimeManager( string name ) : base( name )
		{}

		public override object GetValue()
		{
			var result = HttpContext.Current.Transform( x => x.Items[ Key ] );
			return result;
		}

		public override void RemoveValue() 
		{ 
			HttpContext.Current.NotNull( x => x.Items.Remove( Key ) );
		} 

		public override void SetValue(object newValue) 
		{ 
			HttpContext.Current.Items[ Key ] = newValue; 
		}
	}
}