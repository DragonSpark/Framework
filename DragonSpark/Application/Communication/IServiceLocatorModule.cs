using System.Web;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Communication
{
	public interface IServiceLocatorModule : IHttpModule
	{
		IServiceLocator ServiceLocator { get; }
	}
}