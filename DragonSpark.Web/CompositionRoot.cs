using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace DragonSpark.Web
{
	public class CompositionRoot : IHttpControllerActivator
	{
		readonly IServiceLocator container;

		public CompositionRoot(IServiceLocator container)
		{
			this.container = container;
		}

		public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
		{
			var controller = container.GetInstance( controllerType ).As<IHttpController>( x => request.RegisterForDispose( new Release( x.TryDispose ) ) );
			return controller;
		}

		class Release : IDisposable
		{
			readonly Action release;

			public Release(Action release)
			{
				this.release = release;
			}

			public void Dispose()
			{
				release();
			}
		}
	}
}