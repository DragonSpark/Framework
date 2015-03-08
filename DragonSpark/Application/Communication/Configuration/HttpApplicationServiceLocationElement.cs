using System.Web;

namespace DragonSpark.Application.Communication.Configuration
{
	public sealed class HttpApplicationServiceLocationElement : ServiceLocationElement
	{
        protected override object CreateBehavior()
        {
			var result = new ServiceLocationBehavior( () => HttpContext.Current.ApplicationInstance.GetModule<IServiceLocatorModule>().ServiceLocator );
			return result;
        }
	}
}