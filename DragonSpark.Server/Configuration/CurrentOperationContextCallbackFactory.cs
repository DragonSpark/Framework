using System;
using System.ServiceModel;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Server.Configuration
{
	public class CurrentOperationContextCallbackFactory : GenericItemFactoryBase
	{
		protected override TItem Create<TItem>( IUnityContainer container, string buildName )
		{
			try
			{
				var result = OperationContext.Current.GetCallbackChannel<TItem>();
				return result;
			}
			catch ( NullReferenceException )
			{}
			catch ( InvalidCastException )
			{}

			return default(TItem);
		}
	}
}