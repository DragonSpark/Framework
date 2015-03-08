using System.Collections;
using System.Web;
using DragonSpark.Extensions;

namespace DragonSpark.IoC
{
	public class SessionLifetimeManager : KeyBasedLifetimeManager
	{
		public SessionLifetimeManager( string name ) : base( name )
		{}

		public override object GetValue()
		{
			var result = HttpContext.Current.Session[ Key ];
			return result;
		}

		public override void RemoveValue() 
		{ 
			HttpContext.Current.Session.Remove( Key );
		} 

		public override void SetValue(object newValue) 
		{ 
			HttpContext.Current.Session[ Key ] = newValue; 
		}
	}
}